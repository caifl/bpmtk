using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class ExclusiveGatewayActivityBehavior : GatewayActivityBehavior
    {
        protected override SequenceFlow GetDefaultOutgoing(ExecutionContext executionContext)
        {
            var gateway = executionContext.Node as ExclusiveGateway;

            return gateway.Default;
        }
    }
}
