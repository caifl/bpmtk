using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskInstance
    {
        IDictionary<string, object> ProcessVariables
        {
            get;
        }

        long Id
        {
            get;
        }

        //ProcessDefinition ProcessDefinition
        //{
        //    get;
        //    set;
        //}

        long? ProcessInstanceId
        {
            get;
        }

        long? ActivityInstanceId
        {
            get;
        }

        TaskState State
        {
            get;
        }

        DelegationState? DelegationState
        {
            get;
        }

        DateTime LastStateTime
        {
            get;
        }

        long? TokenId
        {
            get;
        }

        string Name
        {
            get;
        }

        short Priority
        {
            get;
        }

        string ActivityId
        {
            get;
        }

        DateTime Created
        {
            get;
        }

        DateTime? ClaimedTime
        {
            get;
        }

        string Assignee
        {
            get;
        }

        DateTime? DueDate
        {
            get;
            set;
        }

        DateTime Modified
        {
            get;
        }

        string Description
        {
            get;
        }

        object GetVariable(string name);
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
