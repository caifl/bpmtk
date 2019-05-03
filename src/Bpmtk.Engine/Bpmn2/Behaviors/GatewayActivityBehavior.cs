using System;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public abstract class GatewayActivityBehavior : FlowNodeActivityBehavior
    {
        protected override SequenceFlow GetDefaultOutgoing(ExecutionContext executionContext)
        {
            var gateway = executionContext.Node as ExclusiveGateway;

            return gateway.Default;
        }
    }
}
