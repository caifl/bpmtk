using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Utils;
using System;

namespace Bpmtk.Engine.Events
{
    public class EventSubscription
    {
        public EventSubscription()
        {
            this.Created = Clock.Now;
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string EventType
        {
            get;
            set;
        }

        public virtual string EventName
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            set;
        }

        public virtual ProcessDefinition ProcessDefinition
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual Token Token
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual string TenantId
        {
            get;
            set;
        }
    }
}
