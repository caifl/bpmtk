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

        public virtual async Task<TaskInstance> ClaimAsync(long taskId, string comment = null)
        {
            var task = await this.FindAsync(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var date = Clock.Now;

            task.AssigneeId = context.UserId;
            task.ClaimedTime = date;
            task.Modified = date;

            if (comment != null)
                await this.CreateCommentAsync(task, comment);

            await this.session.FlushAsync();

            return task;
        }

        async Task<Comment> CreateCommentAsync(TaskInstance task, string comment)
        {
            var item = new Comment();
            item.Task = task;
            item.UserId = this.context.UserId;
            item.Created = Clock.Now;
            item.Body = comment;

            await this.session.SaveAsync(item);

            return item;
        }

        public virtual async Task<TaskInstance> AssignAsync(long taskId, 
            int assigneeId,
            string comment = null)
        {
            var task = await this.FindAsync(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var date = Clock.Now;

            task.AssigneeId = assigneeId;
            task.ClaimedTime = date;
            task.Modified = date;

            if (comment != null)
                await this.CreateCommentAsync(task, comment);

            await this.session.FlushAsync();

            return task;
        }

        public virtual async Task CompleteAsync(long taskId, 
            IDictionary<string, object> variables = null,
            string comment = null)
        {
            var task = await this.FindAsync(taskId);
            if (task.State != TaskState.Active)
                throw new InvalidOperationException("Invalid state transition.");

            var theToken = task.Token;

            task.State = TaskState.Completed;
            task.LastStateTime = Clock.Now;
            task.Token = null; //Clear token

            if (comment != null)
                await this.CreateCommentAsync(task, comment);

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

        public virtual Task<TaskInstance> FindAsync(long taskId)
        {
            return this.session.FindAsync<TaskInstance>(taskId);
        }

        public virtual Task<IList<AssignmentStrategyEntry>> GetAssignmentStrategyEntries()
        {
            throw new NotImplementedException();
        }

        //public virtual IAssignmentStrategy GetAssignmentStrategy(string key)
        //{
        //    if (key == null)
        //        throw new ArgumentNullException(nameof(key));

        //    var item = this.context.Engine.GetTaskAssignmentStrategy(key);
        //    if (item != null)
        //        return item;

        //    return new DefaultAssignmentStrategy();
        //}

        //public virtual Task RemoveAsync(TaskInstance task)
        //{
        //    return this.session.RemoveAsync(task);
        //}

        public Task<TaskInstance> ResumeAsync(long taskId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task SetNameAsync(long taskId, string name)
        {
            var task = await this.FindAsync(taskId);
            task.Name = name;

            await this.session.FlushAsync();
        }

        public virtual async Task SetPriorityAsync(long taskId, short priority)
        {
            var task = await this.FindAsync(taskId);
            task.Priority = priority;

            await this.session.FlushAsync();
        }

        public Task<TaskInstance> SuspendAsync(long taskId, string comment = null)
        {
            throw new NotImplementedException();
        }
    }
}
