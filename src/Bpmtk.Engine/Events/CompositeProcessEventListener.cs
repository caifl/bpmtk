using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public class CompositeProcessEventListener : IProcessEventListener
    {
        private readonly IEnumerable<IProcessEventListener> processEventListeners;

        public CompositeProcessEventListener(IEnumerable<IProcessEventListener> processEventListeners)
        {
            this.processEventListeners = processEventListeners;
        }

        public virtual async Task ActivityStartAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ActivityStartAsync(executionContext);
        }

        public virtual async Task ActivityEndAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ActivityEndAsync(executionContext);
        }

        public virtual async Task ProcessEndAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ProcessEndAsync(executionContext);
        }

        public virtual async Task ActivityReadyAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ActivityReadyAsync(executionContext);
        }

        public async Task ProcessStartAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ProcessStartAsync(executionContext);
        }

        public async Task TakeTransitionAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.TakeTransitionAsync(executionContext);
        }
    }
}
