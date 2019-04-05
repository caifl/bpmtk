using System;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskInstance
    {
        long Id
        {
            get;
        }

        long? ProcessInstanceId
        {
            get;
        }

        TaskState State
        {
            get;
        }

        DateTime LastStateTime
        {
            get;
        }

        string Name
        {
            get;
            set;
        }

        short Priority
        {
            get;
            set;
        }

        int? AssigneeId
        {
            get;
            set;
        }

        DateTime Created
        {
            get;
        }

        string Description
        {
            get;
            set;
        } 
    }
}
