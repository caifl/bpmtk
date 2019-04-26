using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Engine.Models
{
    public class TaskInstance //: ITaskInstance
    {
        protected ProcessInstance processInstance;
        protected ActivityInstance activityInstance;
        protected bool isSuspended;
        //protected ICollection<IdentityLink> identityLinks;

        //protected TaskInstance()
        //{ }

        //public TaskInstance(Token token)
        //{
        //    this.identityLinks = new List<IdentityLink>();

        //    this.Token = token;
        //    this.processInstance = token.ProcessInstance;
        //    this.activityInstance = token.ActivityInstance;
        //    this.Created = Clock.Now;
        //    this.State = TaskState.Active;
        //    this.ActivityId = token.ActivityId;
        //    this.LastStateTime = this.Created;

        //    this.ProcessInstanceId = this.processInstance?.Id;
        //}
        public virtual ICollection<Variable> Variables
        {
            get;
            set;
        }

        public virtual long Id
        {
            get;
            set;
        }

        //public virtual ProcessDefinition ProcessDefinition
        //{
        //    get;
        //    set;
        //}

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual long? ProcessInstanceId
        {
            get;
            set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get;
            set;
        }

        public virtual TaskState State
        {
            get;
            set;
        }

        public virtual DateTime LastStateTime
        {
            get;
            set;
        }

        public virtual Token Token
        {
            get;
            set;
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
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual DateTime? ClaimedTime
        {
            get;
            set;
        }

        public virtual int? AssigneeId
        {
            get;
            set;
        }

        public virtual User Assignee
        {
            get;
            set;
        }

        public virtual DateTime? DueDate
        {
            get;
            set;
        }

        public virtual DateTime Modified
        {
            get;
            set;
        }

        public virtual string ConcurrencyStamp
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual void AddIdentityLink(int userId, string type)
        {
            var item = new IdentityLink();
            item.User = new User() { Id= userId };
            item.Type = type;

            //this.identityLinks.Add(item);
        }

        public virtual ICollection<IdentityLink> IdentityLinks
        {
            get;
            set;
        }

        //public virtual void Resume(IContext context)
        //{
        //    this.isSuspended = false;
        //    this.Token.Resume(context);
        //}

        //public virtual void Suspend(IContext context)
        //{
        //    this.isSuspended = true;
        //    this.Token.Suspend(context);
        //}

        public virtual bool IsClosed
        {
            get => this.State == TaskState.Completed || this.State == TaskState.Terminated;
        }

        //internal virtual void TerminateInternal(IContext context)
        //{
        //    this.State = TaskState.Terminated;
        //    this.LastStateTime = Clock.Now;
        //}

        //public virtual void Terminate(IContext context, string endReason)
        //{
        //    if (this.IsClosed)
        //        throw new EngineException("The task has closed already.");

        //    if (this.processInstance != null)
        //        this.processInstance.Terminate(context, endReason);
        //}

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

        Suspended = 2,

        Completed = 4,

        Terminated = 8
    }
}
