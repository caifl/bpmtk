using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Events
{
    public class CompositeTaskEventListener : ITaskEventListener
    {
        private readonly IEnumerable<ITaskEventListener> taskEventListeners;

        public CompositeTaskEventListener(IEnumerable<ITaskEventListener> taskEventListeners)
        {
            this.taskEventListeners = taskEventListeners;
        }

        public Task AssignedAsync()
        {
            throw new NotImplementedException();
        }

        public Task CompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreatedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
