using System;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskEvent
    {
        IContext Context
        {
            get;
        }

        ITaskInstance Task
        {
            get;
        }
    }
}
