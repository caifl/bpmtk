using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks
{
    public class TaskManager : ITaskManager
    {
        private readonly Context context;
        private readonly IDbSession session;

        public TaskManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;
        }

        public virtual IQueryable<TaskInstance> Tasks => this.session.Tasks;

        public virtual async Task CompleteAsync(long taskId, 
            IDictionary<string, object> variables = null)
        {
            var task = await this.FindTaskAsync(taskId);
            if (task.State != TaskState.Active)
                throw new InvalidOperationException("Invalid state transition.");

            var theToken = task.Token;

            task.State = TaskState.Completed;
            task.LastStateTime = Clock.Now;
            task.Token = null; //Clear token

            //await this.db.UpdateAsync(task);
            await this.session.FlushAsync();

            if (theToken != null)
            {
                var executionContext = ExecutionContext.Create(context, theToken);
                await executionContext.SignalAsync(null, null);
            }
        }

        public virtual async Task CreateAsync(TaskInstance task)
        {
            await this.session.SaveAsync(task);
            await this.session.FlushAsync();
        }

        public virtual ITaskInstanceBuilder CreateBuilder()
            => new TaskInstanceBuilder(this.context);

        public virtual ITaskQuery CreateQuery()
            => new TaskQuery(this.session);

        public virtual Task<TaskInstance> FindTaskAsync(long taskId)
        {
            return this.session.FindAsync<TaskInstance>(taskId);
        }

        public virtual ITaskAssignmentStrategy GetTaskAssignmentStrategy(string name)
        {
            return new TaskAssignmentStrategy();
        }

        public virtual Task RemoveAsync(TaskInstance task)
        {
            return this.session.RemoveAsync(task);
        }

        public virtual Task UpdateAsync(TaskInstance task)
        {
            return this.session.UpdateAsync(task);
        }
    }
}
