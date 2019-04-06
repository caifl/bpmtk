using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks
{
    public class TaskInstance : ITaskInstance
    {
        protected ProcessInstance processInstance;
        protected ActivityInstance activityInstance;
        protected bool isSuspended;

        protected TaskInstance()
        { }

        public TaskInstance(Token token)
        {
            this.Token = token;
            this.processInstance = token.ProcessInstance;
            this.activityInstance = token.ActivityInstance;
            this.Created = Clock.Now;
            this.State = TaskState.Active;
            this.ActivityId = token.ActivityId;
            this.LastStateTime = this.Created;

            this.ProcessInstanceId = this.processInstance?.Id;
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get => this.processInstance;
            protected set
            {
                this.processInstance = value;
            }
        }

        public virtual long? ProcessInstanceId
        {
            get;
            protected set;
        }

        public virtual TaskState State
        {
            get;
            protected set;
        }

        public virtual DateTime LastStateTime
        {
            get;
            protected set;
        }

        public virtual Token Token
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual short Priority
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual DateTime Created
        {
            get;
            protected set;
        }

        public virtual DateTime? ClaimedTime
        {
            get;
            protected set;
        }

        public virtual int? AssigneeId
        {
            get;
            set;
        }

        public virtual User Assignee
        {
            get;
            protected set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual void Resume()
        {
            this.isSuspended = false;
            this.Token.Resume();
        }

        public virtual void Suspend()
        {
            this.isSuspended = true;
            this.Token.Suspend();
        }

        public virtual void Complete(IContext context, IDictionary<string, object> variables = null)
        {
            if (this.State != TaskState.Active)
                throw new InvalidOperationException("Invalid state transition.");

            var theToken = this.Token;

            this.State = TaskState.Completed;
            this.LastStateTime = Clock.Now;
            this.Token = null; //Clear token

            var store = context.GetService<ITaskStore>();
            store.Update(this);

            if (theToken != null)
            {
                var executionContext = ExecutionContext.Create(context, theToken);
                executionContext.LeaveNode();
            }
        }

        public virtual object GetVariable(string name)
        {
            var act = this.activityInstance;
            if (act != null)
                return act.GetVariable(name);

            return null;
        }
    }

    public enum TaskState
    {
        Ready = 0,

        Active = 1,

        Completed = 2
    }
}
