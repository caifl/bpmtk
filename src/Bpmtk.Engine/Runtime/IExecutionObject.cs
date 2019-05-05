using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Runtime
{
    public interface IExecutionObject
    {
        long Id
        {
            get;
        }

        string Name
        {
            get;
        }

        ExecutionState State
        {
            get;
        }

        DateTime Created
        {
            get;
        }

        DateTime? StartTime
        {
            get;
        }

        DateTime LastStateTime
        {
            get;
        }

        string Description
        {
            get;
        }

        IDictionary<string, object> Variables
        {
            get;
        }

        object GetVariable(string name);
    }

    public enum ExecutionState : int
    {
        Ready = 0,

        Active = 1,

        Suspended = 2,

        Completed = 4,

        Aborted = 8,

        Terminated = 16
    }
}
