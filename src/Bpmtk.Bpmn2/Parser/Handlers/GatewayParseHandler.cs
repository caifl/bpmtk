using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    abstract class GatewayParseHandler : FlowNodeParseHandler
    {
        protected virtual void Init(Gateway gateway, IParseContext context, XElement element)
        {
            base.Init(gateway, context, element);

            var value = element.GetAttribute("gatewayDirection");
            if (value != null)
                gateway.GatewayDirection = (GatewayDirection)Enum.Parse(typeof(GatewayDirection), value);

            var defaultOutgoing = element.GetAttribute("default");
            if (defaultOutgoing != null)
            {
                context.AddReferenceRequest<SequenceFlow>(defaultOutgoing, (s) => gateway.Default = s);
            }       
        }
    }

    class ExclusiveGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var gateway = context.BpmnFactory.CreateExclusiveGateway();
            parent.FlowElements.Add(gateway);

            base.Init(gateway, context, element);

            return gateway;
        }
    }

    class InclusiveGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var gateway = context.BpmnFactory.CreateInclusiveGateway();
            parent.FlowElements.Add(gateway);

            base.Init(gateway, context, element);

            return gateway;
        }
    }

    class ParallelGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var gateway = context.BpmnFactory.CreateParallelGateway();
            parent.FlowElements.Add(gateway);

            base.Init(gateway, context, element);

            return gateway;
        }
    }
}
