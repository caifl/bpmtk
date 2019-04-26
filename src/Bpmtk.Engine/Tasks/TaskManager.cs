using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks
{
    class TaskManager : ITaskManager
    {
        private readonly Context context;
        private readonly IDbSession db;

        public TaskManager(Context context)
        {
            this.context = context;
            this.db = context.DbSession;
        }

        public virtual IQueryable<TaskInstance> Tasks => this.db.Tasks;

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
            await this.db.FlushAsync();

            if (theToken != null)
            {
                var executionContext = ExecutionContext.Create(context, theToken);
                await executionContext.SignalAsync(null, null);
            }
        }

        public virtual Task CreateAsync(TaskInstance task)
        {
            return this.db.SaveAsync(task);
        }

        public virtual ITaskQuery CreateQuery()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TaskInstance> FindTaskAsync(long taskId)
        {
            return this.db.FindAsync<TaskInstance>(taskId);
        }

        public virtual Task RemoveAsync(TaskInstance task)
        {
            return this.db.RemoveAsync(task);
        }

        public virtual Task UpdateAsync(TaskInstance task)
        {
            return this.db.UpdateAsync(task);
        }
    }
}
