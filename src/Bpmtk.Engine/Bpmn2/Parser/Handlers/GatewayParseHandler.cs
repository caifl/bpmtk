using Bpmtk.Bpmn2;
using System;
using System.Xml.Linq;
using Bpmtk.Engine.Bpmn2.Behaviors;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
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

    class ComplexGatewayParseHandler : GatewayParseHandler
    {
        public ComplexGatewayParseHandler()
        {
            this.handlers.Add("activationCondition", new ExpressionParseHandler<ComplexGateway>((p, r) =>
            {
                p.ActivationCondition = r;
            }));
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var gateway = context.BpmnFactory.CreateComplexGateway();

            parent.FlowElements.Add(gateway);

            base.Init(gateway, context, element);

            return gateway;
        }
    }

    class EventBasedGatewayParseHandler : GatewayParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var gateway = context.BpmnFactory.CreateEventBasedGateway();
            gateway.Instantiate = element.GetBoolean("instantiate");
            gateway.EventGatewayType = element.GetEnum<EventBasedGatewayType>("eventGatewayType", EventBasedGatewayType.Exclusive);

            parent.FlowElements.Add(gateway);

            base.Init(gateway, context, element);

            return gateway;
        }
    }
}
