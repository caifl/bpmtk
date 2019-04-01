using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    public abstract class ExecutionObject
    {
        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual ExecutionState State
        {
            get;
            protected set;
        }

        public abstract IEnumerable<VariableInstance> VariableInstances
        {
            get;
        }

        public virtual VariableInstance GetVariableInstance(string name)
        {
            return null;
        }

        public virtual object GetVariable(string name)
        {
            return null;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            protected set;
        }

        public virtual DateTime? StartTime
        {
            get;
            protected set;
        }

        public virtual DateTime? EndTime
        {
            get;
            protected set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual void Suspend()
        {

        }

        public virtual void Resume()
        {

        }

    }

    public enum ExecutionState : int
    {
        Inactive = 0,

        Active = 1,

        Suspended = 2,

        Completed = 4,

        Aborted = 8
    }
}
