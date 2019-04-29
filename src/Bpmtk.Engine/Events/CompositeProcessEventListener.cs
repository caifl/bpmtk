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

        public virtual async Task ActivatedAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.ActivatedAsync(executionContext);
        }

        public virtual async Task EndedAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.EndedAsync(executionContext);
        }

        public virtual async Task EnterNodeAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.EnterNodeAsync(executionContext);
        }

        public async Task LeaveNodeAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.LeaveNodeAsync(executionContext);
        }

        public async Task StartedAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.StartedAsync(executionContext);
        }

        public async Task TakeTransitionAsync(IExecutionContext executionContext)
        {
            foreach (var item in this.processEventListeners)
                await item.TakeTransitionAsync(executionContext);
        }
    }
}
