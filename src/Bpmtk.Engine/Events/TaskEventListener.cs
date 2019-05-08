using System;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Events
{
    public abstract class TaskEventListener : ITaskEventListener
    {
        public virtual void Assigned(ITaskEvent taskEvent)
        {
            
        }

        public virtual void Claimed(ITaskEvent taskEvent)
        {
            
        }

        public virtual void Completed(ITaskEvent taskEvent)
        {
            
        }

        public virtual void Created(ITaskEvent taskEvent)
        {
            
        }

        public virtual void Delegated(ITaskEvent taskEvent)
        {

        }

        public virtual void Resumed(ITaskEvent taskEvent)
        {
            
        }

        public virtual void Suspended(ITaskEvent taskEvent)
        {
            
        }
    }
}
