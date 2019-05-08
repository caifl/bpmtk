using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks
{
    public class TaskManager : ITaskManager
    {
        private readonly Context context;
        private readonly IDbSession session;
        protected CompositeTaskEventListener taskEventListener;

        public TaskManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;

            var options = this.context.Engine.Options;
            this.taskEventListener = new CompositeTaskEventListener(options.TaskEventListeners);
        }

        public virtual CompositeTaskEventListener CompositeTaskEventListener
        {
            get => this.taskEventListener;
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

        //public virtual TaskInstance Complete(long taskId, 
        //    IDictionary<string, object> variables = null,
        //    string comment = null)
        //{
        //    var task = this.Find(taskId);
        //    if (task.State != TaskState.Active)
        //        throw new InvalidOperationException("Invalid state transition.");

        //    var theToken = task.Token;

        //    task.State = TaskState.Completed;
        //    task.LastStateTime = Clock.Now;
        //    task.Token = null; //Clear token

        //    if (comment != null)
        //        this.CreateComment(task, comment);

        //    this.session.Flush();

        //    if (theToken != null)
        //    {
        //        var ecm = this.context.ExecutionContextManager;
        //        var executionContext = ecm.GetOrCreate(theToken);
        //        executionContext.Trigger();
        //    }

        //    return task;
        //}

        public virtual async Task<ITaskInstance> CompleteAsync(long taskId,
            IDictionary<string, object> variables = null,
            string comment = null)
        {
            var task = await this.FindAsync(taskId);
            if (task.State != TaskState.Active || task.Token == null)
                throw new StateTransitionNotAllowedException("Invalid state transition.");

            var token = task.Token;

            //Change task-state.
            task.State = TaskState.Completed;
            task.LastStateTime = Clock.Now;
            task.Token = null; //Clear token

            if (comment != null)
                this.CreateComment(task, comment);

            //Update variables.
            if (variables != null && variables.Count > 0)
            {
                var em = variables.GetEnumerator();
                while (em.MoveNext())
                {
                    var name = em.Current.Key;
                    var value = em.Current.Value;

                    task.SetVariable(name, value);
                }
            }

            //Save changes.
            await this.session.FlushAsync();

            //Fire taskCompletedEvent.
            this.taskEventListener.Completed(new TaskCompletedEvent(this.context, task));

            var executionContext = this.context.ExecutionContextManager
                .GetOrCreate(token);

            //Trigger token to leave current node.
            executionContext.Trigger();

            return task;
        }

        //public virtual void CreateAsync(TaskInstance task)
        //{
        //    this.session.Save(task);
        //    this.session.Flush();
        //}

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

        #region Comments

        public virtual async Task<IList<Comment>> GetCommentsAsync(long taskId)
        {
            var query = this.session.Query<Comment>()
                .Where(x => x.Task.Id == taskId)
                .OrderByDescending(x => x.Created);

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task RemoveCommentsAsync(params long[] commentIds)
        {
            var query = this.session.Query<Comment>()
                .Where(x => commentIds.Contains(x.Id));

            var items = await this.session.QueryMultipleAsync(query);
            this.session.RemoveRange(items);
            await this.session.FlushAsync();
        }

        #endregion

        #region IdentityLinks Management

        public virtual Task<IList<IdentityLink>> GetIdentityLinksAsync(long taskId)
        {
            var query = this.session.IdentityLinks;
            query = query.Where(x => x.Task.Id == taskId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<IList<IdentityLink>> AddUserLinksAsync(long taskId, IEnumerable<string> userIds, string type)
        {
            if (userIds == null)
                throw new ArgumentNullException(nameof(userIds));

            if (!userIds.Any())
                throw new ArgumentException(nameof(userIds));

            var task = await this.FindAsync(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var list = new List<IdentityLink>();
            foreach (var userId in userIds)
            {
                var item = new IdentityLink();
                item.Task = task;
                item.UserId = userId;
                item.Type = type;
                item.Created = Clock.Now;

                list.Add(item);
            }

            await this.session.SaveRangeAsync(list);
            await this.session.FlushAsync();

            return list;
        }

        public virtual async Task<IList<IdentityLink>> AddGroupLinksAsync(long taskId,
            IEnumerable<string> groupIds, string type)
        {
            if (groupIds == null)
                throw new ArgumentNullException(nameof(groupIds));

            if (!groupIds.Any())
                throw new ArgumentException(nameof(groupIds));

            var task = await this.FindAsync(taskId);
            if (task == null)
                throw new ObjectNotFoundException(nameof(TaskInstance));

            var list = new List<IdentityLink>();
            foreach (var group in groupIds)
            {
                var item = new IdentityLink();
                item.Task = task;
                item.GroupId = group;
                item.Created = Clock.Now;

                list.Add(item);
            }

            await this.session.SaveRangeAsync(list);
            await this.session.FlushAsync();

            return list;
        }

        public virtual async Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds)
        {
            //check process-instance state.

            //fetch items to be deleted.
            var query = this.session.IdentityLinks
                .Where(x => x.ProcessInstance.Id == processInstanceId
                && identityLinkIds.Contains(x.Id));

            var items = query.ToList();
            if (items.Count > 0)
            {
                this.session.RemoveRange(items);
                await this.session.FlushAsync();
            }
        }

        #endregion

        public Task<TaskInstance> ResumeAsync(long taskId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<ITaskInstance> SetNameAsync(long taskId, string name)
        {
            var task = await this.FindAsync(taskId);
            task.Name = name;

            await this.session.FlushAsync();

            return task;
        }

        public virtual async Task<ITaskInstance> SetPriorityAsync(long taskId, short priority)
        {
            var task = await this.FindAsync(taskId);
            task.Priority = priority;

            await this.session.FlushAsync();

            return task;
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
    }
}
