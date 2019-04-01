using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Tasks
{
    public class TaskInstance
    {
        protected ProcessInstance processInstance;
        protected Token token;
        protected ActivityInstance activityInstance;
        protected bool isSuspended;

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual User Assignee
        {
            get;
            protected set;
        }

        public virtual void Resume()
        {
            this.isSuspended = false;
            this.token.Resume();
        }

        public virtual void Suspend()
        {
            this.isSuspended = true;
            this.token.Suspend();
        }

        public virtual void Complete()
        {
            if (this.token != null)
                this.token.Signal();
        }
    }
}
