using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class GatewayHandler<TFlowElementContainer, TGateway> : FlowNodeHandler<TFlowElementContainer, TGateway>
        where TGateway : Gateway
        where TFlowElementContainer : IFlowElementsContainer
    {
        public GatewayHandler()
        {
        }

        public override TGateway Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var gateway = base.Create(parent, context, element);

            var value = element.GetAttribute("gatewayDirection");
            if (value != null)
                gateway.GatewayDirection = (GatewayDirection)Enum.Parse(typeof(GatewayDirection), value);

            var defaultOutgoing = element.GetAttribute("default");
            if (defaultOutgoing != null)
            {
                var scope = context.PeekScope();
                scope.AddDefault(defaultOutgoing, gateway);
            }

            return gateway;
        }
    }

    class ExclusiveGatewayHandler<TFlowElementContainer> : GatewayHandler<TFlowElementContainer, ExclusiveGateway>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override ExclusiveGateway Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            return evnt;
        }

        protected override ExclusiveGateway New(IParseContext context, XElement element) => context.BpmnFactory.CreateExclusiveGateway();
    }

    class InclusiveGatewayHandler<TFlowElementContainer> : GatewayHandler<TFlowElementContainer, InclusiveGateway>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override InclusiveGateway Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            return evnt;
        }

        protected override InclusiveGateway New(IParseContext context, XElement element) => context.BpmnFactory.CreateInclusiveGateway();
    }

    class ParallelGatewayHandler<TFlowElementContainer> : GatewayHandler<TFlowElementContainer, ParallelGateway>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override ParallelGateway Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            return evnt;
        }

        protected override ParallelGateway New(IParseContext context, XElement element) => context.BpmnFactory.CreateParallelGateway();
    }
}
