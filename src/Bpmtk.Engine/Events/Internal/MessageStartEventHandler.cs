using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Events
{
    class MessageStartEventHandler : IMessageStartEventHandler
    {
        public async Task<ProcessInstance> ExecuteAsync(IContext context,
            EventSubscription eventSubscription, 
            IDictionary<string, object> messageData)
        {
            var processDefinition = eventSubscription.ProcessDefinition;
            var model = await context.DeploymentManager.GetBpmnModelAsync(processDefinition.DeploymentId);
            var flowNode = model.GetFlowElement(eventSubscription.ActivityId) 
                as FlowNode;

            var builder = context.RuntimeManager.CreateInstanceBuilder();
            builder.SetProcessDefinition(processDefinition);

            var pi = await context.RuntimeManager.StartProcessAsync(builder);

            return pi;
        }
    }
}
