using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    class MessageStartEventHandler : IMessageStartEventHandler
    {
        public virtual async Task<IProcessInstance> ExecuteAsync(IContext context,
            EventSubscription eventSubscription, 
            IDictionary<string, object> messageData)
        {
            var runtimeManager = context.RuntimeManager;

            var processDefinition = eventSubscription.ProcessDefinition;
            var model = context.DeploymentManager.GetBpmnModel(processDefinition.DeploymentId);
            var flowNode = model.GetFlowElement(eventSubscription.ActivityId)
                as FlowNode;

            var builder = runtimeManager.CreateInstanceBuilder();
            //builder.SetProcessDefinition(processDefinition);

            var pi = await runtimeManager.StartProcessAsync(builder);
            return pi;
        }
    }
}
