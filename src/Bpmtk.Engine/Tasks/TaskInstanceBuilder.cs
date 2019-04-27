using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks
{
    public class TaskInstanceBuilder : ITaskInstanceBuilder
    {
        protected Token token;
        protected string activityId;
        protected string name;
        protected short? priority;
        protected User assignee;
        protected DateTime? dueDate;

        public TaskInstanceBuilder(Context context)
        {
            Context = context;
            this.DbSession = context.DbSession;
        }

        public virtual Context Context { get; }

        public virtual IDbSession DbSession
        {
            get;
        }

        IContext ITaskInstanceBuilder.Context => this.Context;

        public virtual async Task<TaskInstance> BuildAsync()
        {
            var date = Clock.Now;

            var task = new TaskInstance();
            task.Name = this.name;
            task.ActivityId = this.activityId;
            task.Created = date;
            task.State = TaskState.Active;
            task.LastStateTime = date;
            task.Modified = date;
            task.DueDate = this.dueDate;

            if (this.priority.HasValue)
                task.Priority = this.priority.Value;

            task.Assignee = this.assignee;

            if (this.token != null)
            {
                task.ActivityInstance = this.token.ActivityInstance;
                task.ProcessInstance = this.token.ProcessInstance;
                task.Token = this.token;
            }

            await this.DbSession.SaveAsync(task);
            await this.DbSession.FlushAsync();

            return task;
        }

        public virtual ITaskInstanceBuilder SetActivityId(string activityId)
        {
            this.activityId = activityId;

            return this;
        }

        public virtual ITaskInstanceBuilder SetAssignee(User assignee)
        {
            this.assignee = assignee;

            return this;
        }

        public virtual ITaskInstanceBuilder SetDueDate(DateTime dueDate)
        {
            this.dueDate = dueDate;

            return this;
        }

        public virtual ITaskInstanceBuilder SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual ITaskInstanceBuilder SetPriority(short priority)
        {
            this.priority = priority;

            return this;
        }

        public virtual ITaskInstanceBuilder SetToken(Token token)
        {
            this.token = token;

            return this;
        }
    }
}
