using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual TaskInstance Claim(long taskId, string comment = null)
        {
            var task = this.Find(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var date = Clock.Now;

            task.Assignee = context.UserId;
            task.ClaimedTime = date;
            task.Modified = date;

            if (comment != null)
                this.CreateComment(task, comment);

            this.session.Flush();

            return task;
        }

        Comment CreateComment(TaskInstance task, string comment)
        {
            var item = new Comment();
            item.Task = task;
            item.UserId = this.context.UserId;
            item.Created = Clock.Now;
            item.Body = comment;

            this.session.Save(item);

            return item;
        }

        public virtual TaskInstance Assign(long taskId, 
            string assignee,
            string comment = null)
        {
            var task = this.Find(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var date = Clock.Now;

            task.Assignee = assignee;
            task.ClaimedTime = date;
            task.Modified = date;

            if (comment != null)
                this.CreateComment(task, comment);

            this.session.Flush();

            return task;
        }

        public virtual TaskInstance Complete(long taskId, 
            IDictionary<string, object> variables = null,
            string comment = null)
        {
            var task = this.Find(taskId);
            if (task.State != TaskState.Active)
                throw new InvalidOperationException("Invalid state transition.");

            var theToken = task.Token;

            task.State = TaskState.Completed;
            task.LastStateTime = Clock.Now;
            task.Token = null; //Clear token

            if (comment != null)
                this.CreateComment(task, comment);

            this.session.Flush();

            if (theToken != null)
            {
                var executionContext = ExecutionContext.Create(context, theToken);
                executionContext.Trigger(null, null);
            }

            return task;
        }

        public virtual async Task<ITaskInstance> CompleteAsync(long taskId,
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
                this.CreateComment(task, comment);

            await this.session.FlushAsync();

            if (theToken != null)
            {
                var executionContext = ExecutionContext.Create(context, theToken);
                executionContext.Trigger(null, null);
            }

            return task;
        }

        public virtual void CreateAsync(TaskInstance task)
        {
            this.session.Save(task);
            this.session.Flush();
        }

        public virtual ITaskInstanceBuilder CreateBuilder()
            => new TaskInstanceBuilder(this.context);

        public virtual TaskQuery CreateQuery()
            => new TaskQuery(this.session);

        ITaskQuery ITaskManager.CreateQuery() => this.CreateQuery();

        public virtual TaskInstance Find(long taskId)
        {
            return this.session.Find<TaskInstance>(taskId);
        }

        public virtual async Task<TaskInstance> FindAsync(long taskId)
        {
            return await this.session.FindAsync<TaskInstance>(taskId);
        }

        async Task<ITaskInstance> ITaskManager.FindAsync(long taskId) => await this.FindAsync(taskId);

        public virtual IAssignmentStrategy GetAssignmentStrategy(string key)
        {
            var engineOptions = this.context.Engine.Options;
            AssignmentStrategyEntry item = null;

            if (engineOptions.AssignmentStrategyEntries.TryGetValue(key, out item))
                return item.AssignmentStrategy;

            return null;
        }

        public virtual IReadOnlyList<AssignmentStrategyEntry> GetAssignmentStrategyEntries()
        {
            var engineOptions = this.context.Engine.Options;
            var list = new List<AssignmentStrategyEntry>(engineOptions.AssignmentStrategyEntries.Values);
            return list.AsReadOnly();
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

        public virtual void SetNameAsync(long taskId, string name)
        {
            var task = this.Find(taskId);
            task.Name = name;

            this.session.Flush();
        }

        public virtual void SetPriorityAsync(long taskId, short priority)
        {
            var task = this.Find(taskId);
            task.Priority = priority;

            this.session.Flush();
        }

        public Task<TaskInstance> SuspendAsync(long taskId, string comment = null)
        {
            throw new NotImplementedException();
        }

        ITaskInstanceBuilder ITaskManager.CreateBuilder()
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Find(long taskId)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Claim(long taskId, string comment)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Assign(long taskId, string assignee, string comment)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Suspend(long taskId, string comment)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Resume(long taskId, string comment)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.Complete(long taskId, IDictionary<string, object> variables, string comment)
            => this.Complete(taskId, variables, comment);

        ITaskInstance ITaskManager.SetName(long taskId, string name)
        {
            throw new NotImplementedException();
        }

        ITaskInstance ITaskManager.SetPriority(long taskId, short priority)
        {
            throw new NotImplementedException();
        }

        IReadOnlyList<AssignmentStrategyEntry> ITaskManager.GetAssignmentStrategyEntries()
        {
            throw new NotImplementedException();
        }

        IAssignmentStrategy ITaskManager.GetAssignmentStrategy(string key)
        {
            throw new NotImplementedException();
        }
    }
}
