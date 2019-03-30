using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    abstract class GatewayParseHandler : FlowElementParseHandler
    {
        public GatewayParseHandler()
        {

        }

        protected virtual void Parse(Gateway gateway, IParseContext context, XElement element)
        {
            var value = element.GetAttribute("gatewayDirection");
            if (value != null)
                gateway.GatewayDirection = (GatewayDirection)Enum.Parse(typeof(GatewayDirection), value);

            var defaultOutgoing = element.GetAttribute("default");
            if (defaultOutgoing != null)
            {
                context.AddReferenceRequest(defaultOutgoing, (SequenceFlow target) => gateway.Default = target);
            }
        }
    }

    class ExclusiveGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var evnt = context.BpmnFactory.CreateExclusiveGateway();

            return evnt;
        }
    }

    class InclusiveGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            return context.BpmnFactory.CreateInclusiveGateway();
        }
    }

    class ParallelGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var evnt = context.BpmnFactory.CreateParallelGateway();

            return evnt;
        }
    }
}
