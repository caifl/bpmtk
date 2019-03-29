using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class EventHandler<TFlowElementContainer, TEvent> : FlowNodeHandler<TFlowElementContainer, TEvent>
        where TEvent : Event
        where TFlowElementContainer : IFlowElementsContainer
    {
        public EventHandler()
        {
            this.handlers.Add("property", new PropertyHandler<TEvent>());
        }

        public override TEvent Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            return evnt;
        }
    }

    abstract class CatchEventHandler<TFlowElementContainer, TCatchEvent> : EventHandler<TFlowElementContainer, TCatchEvent>
        where TCatchEvent : CatchEvent
        where TFlowElementContainer : IFlowElementsContainer
    {
        public CatchEventHandler()
        {
            //this.handlers.Add("terminateEventDefinition",)
            this.handlers.Add("dataOutput", new DataOutputHandler<TCatchEvent>());
            this.handlers.Add("dataOutputAssociation", new DataOutputAssociationHandler<TCatchEvent>());

            this.handlers.Add("eventDefinitionRef", new BpmnHandlerCallback<TCatchEvent>((p,c,x) => {
                var eventDefinitionRef = x.Value;

                p.EventDefinitionRefs.Add(eventDefinitionRef);

                return eventDefinitionRef;
            }));
        }

        public override TCatchEvent Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            evnt.ParallelMultiple = element.GetBoolean("parallelMultiple");

            return evnt;
        }
    }

    abstract class ThrowEventHandler<TFlowElementContainer, TThrowEvent> : EventHandler<TFlowElementContainer, TThrowEvent>
        where TThrowEvent : ThrowEvent
        where TFlowElementContainer : IFlowElementsContainer
    {
        public ThrowEventHandler()
        {
            this.handlers.Add("dataInput", new DataInputHandler<TThrowEvent>());
            this.handlers.Add("dataInputAssociation", new DataInputAssociationHandler<TThrowEvent>());

            this.handlers.Add("eventDefinitionRef", new BpmnHandlerCallback<TThrowEvent>((p, c, x) => {
                var eventDefinitionRef = x.Value;

                p.EventDefinitionRefs.Add(eventDefinitionRef);

                return eventDefinitionRef;
            }));
        }

        public override TThrowEvent Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            return evnt;
        }
    }

    class StartEventHandler<TFlowElementContainer> : CatchEventHandler<TFlowElementContainer, StartEvent>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override StartEvent Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var evnt = base.Create(parent, context, element);

            evnt.IsInterrupting = element.GetBoolean("isInterrupting");

            return evnt;
        }

        protected override StartEvent New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateStartEvent();
    }

    class EndEventHandler<TFlowElementContainer> : ThrowEventHandler<TFlowElementContainer, EndEvent>
        where TFlowElementContainer : IFlowElementsContainer
    {
        protected override EndEvent New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateEndEvent();
    }
}
