using Bpmtk.Engine.Utils;
using System;

namespace Bpmtk.Engine.Runtime
{
    public class HistoricToken
    {
        public HistoricToken()
        {
        }

        public HistoricToken(ExecutionContext executionContext, string eventName)
        {
            var token = executionContext.Token;

            this.TokenId = token.Id;
            this.ParentId = token.Parent?.Id;
            this.Name = token.Node?.Name;
            this.ActivityId = token.ActivityId;
            this.TransitionId = executionContext.Transition?.Id;
            this.IsActive = token.IsActive;
            this.IsSuspended = token.IsSuspended;
            this.IsLoopActivity = token.IsMIRoot;
            this.Event = eventName;
            this.Created = Clock.Now;

            this.ProcessInstanceId = token.ProcessInstance.Id;
            this.ActivityInstanceId = token.ActivityInstance?.Id;
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual string Event
        {
            get;
            set;
        }

        public virtual long TokenId
        {
            get;
            protected set;
        }

        public virtual long? ParentId
        {
            get;
            protected set;
        }

        public virtual string TransitionId
        {
            get;
            protected set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual long ProcessInstanceId
        {
            get;
            protected set;
        }

        public virtual long? ActivityInstanceId
        {
            get;
            protected set;
        }

        public virtual bool IsActive
        {
            get;
            protected set;
        }

        public virtual bool IsSuspended
        {
            get;
            protected set;
        }

        public virtual bool IsLoopActivity
        {
            get;
            protected set;
        }

        public virtual DateTime Created//? StartTime
        {
            get;
            protected set;
        }

        //public virtual DateTime? EndTime
        //{
        //    get;
        //    protected set;
        //}
    }
}
