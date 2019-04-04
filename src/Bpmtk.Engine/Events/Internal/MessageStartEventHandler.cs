using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events.Internal
{
    class MessageStartEventHandler : IMessageStartEventHandler
    {
        private readonly IProcessInstanceStore store;

        public MessageStartEventHandler(IProcessInstanceStore store)
        {
            this.store = store;
        }

        public async Task<ProcessInstance> Execute(EventSubscription eventSubscription, object messageData)
        {
            var processDefinition = eventSubscription.ProcessDefinition;

            var processInstance = new ProcessInstance(processDefinition);

            await this.store.SaveAsync(processInstance);

            processInstance.Start(Context.Current, eventSubscription.ActivityId);

            return processInstance;
        }
    }
}
