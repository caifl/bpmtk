using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Events
{
    public class CompositeTaskEventListener : ITaskEventListener
    {
        private readonly IEnumerable<ITaskEventListener> taskEventListeners;

        public CompositeTaskEventListener(IEnumerable<ITaskEventListener> taskEventListeners)
        {
            this.taskEventListeners = taskEventListeners;
        }

        public void Assigned(ITaskEvent taskEvent)
        {
        }

        public void Claimed(ITaskEvent taskEvent)
        {
        }

        public void Completed(ITaskEvent taskEvent)
        {
        }

        public void Created(ITaskEvent taskEvent)
        {
        }

        public void Delegated(ITaskEvent taskEvent)
        {
        }

        public void Resumed(ITaskEvent taskEvent)
        {
        }

        public void Suspended(ITaskEvent taskEvent)
        {
        }
    }
}
