using System;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Events
{
    public interface ITaskEventListener
    {
        void Created(ITaskEvent taskEvent);

        void Assigned(ITaskEvent taskEvent);

        void Claimed(ITaskEvent taskEvent);

        void Suspended(ITaskEvent taskEvent);

        void Resumed(ITaskEvent taskEvent);

        void Completed(ITaskEvent taskEvent);

        void Delegated(ITaskEvent taskEvent);
    }
}
