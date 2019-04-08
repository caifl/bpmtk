using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.Events.Internal
{
    class MessageStartEventHandler : IMessageStartEventHandler
    {
        private readonly IInstanceStore store;
        private IDeploymentManager deploymentManager;

        public MessageStartEventHandler(IDeploymentManager deploymentManager, IInstanceStore store)
        {
            this.deploymentManager = deploymentManager;
            this.store = store;
        }

        public async Task<ProcessInstance> Execute(EventSubscription eventSubscription, object messageData)
        {
            var processDefinition = eventSubscription.ProcessDefinition;
            var model = this.deploymentManager.GetBpmnModel(processDefinition.DeploymentId);
            var flowNode = model.GetFlowElement(eventSubscription.ActivityId) 
                as Bpmn2.FlowNode;

            var processInstance = new ProcessInstance(processDefinition);

            await this.store.SaveAsync(processInstance);

            processInstance.Start(Context.Current, flowNode);

            return processInstance;
        }
    }
}
