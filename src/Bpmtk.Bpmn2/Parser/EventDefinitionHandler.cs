using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class EventDefinitionHandler : BaseElementHandler<Definitions, EventDefinition>
    {
        public static readonly string[] Keys = new string[] {
            "messageEventDefinition",
            "signalEventDefinition",
            "timerEventDefinition",
            "compensateEventDefinition",
            "conditionalEventDefinition",
            "terminateEventDefinition",
            "cancelEventDefinition",
            "errorEventDefinition",
            "escalationEventDefinition",
            "linkEventDefinition",
            "eventDefinition"
            };

        public EventDefinitionHandler()
        {
            this.handlers.Add("source", new BpmnHandlerCallback<EventDefinition>((p, c, x) =>
            {
                var source = x.Value;
                var linkEvent = ((LinkEventDefinition)p);
                linkEvent.Source.Add(source);

                return source;
            }));

            this.handlers.Add("condition", new ExpressionHandler<EventDefinition>((p, c, x, expr) =>
            {
                var conditional = ((ConditionalEventDefinition)p);
                conditional.Condition = expr;
            }));

            this.handlers.Add("timeDuration", new ExpressionHandler<EventDefinition>((p, c, x, expr) => {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeDuration = expr;
            }));

            this.handlers.Add("timeDate", new ExpressionHandler<EventDefinition>((p, c, x, expr) => {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeDate = expr;
            }));

            this.handlers.Add("timeCycle", new ExpressionHandler<EventDefinition>((p, c, x, expr) => {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeCycle = expr;
            }));
        }

        public override EventDefinition Create(Definitions parent, IParseContext context, XElement element)
        {
            var eventDefinition = base.Create(parent, context, element);

            parent.RootElements.Add(eventDefinition);

            return eventDefinition;
        }

        protected override EventDefinition New(IParseContext context, XElement element)
        {
            EventDefinition eventDefinition = null;
            var localName = Helper.GetRealLocalName(element);

            switch (localName)
            {
                case "cancelEventDefinition":
                    eventDefinition = new CancelEventDefinition();
                    break;

                case "errorEventDefinition":
                    eventDefinition = new ErrorEventDefinition()
                    {
                        ErrorRef = element.GetAttribute("errorRef")
                    };
                    break;

                case "timerEventDefinition":
                    eventDefinition = new TimerEventDefinition();
                    break;

                case "terminateEventDefinition":
                    eventDefinition = new TerminateEventDefinition();
                    break;

                case "messageEventDefinition":
                    eventDefinition = new MessageEventDefinition()
                    {
                        OperationRef = element.GetAttribute("operationRef"),
                        MessageRef = element.GetAttribute("messageRef")
                    };
                    break;

                case "conditionalEventDefinition":
                    eventDefinition = new ConditionalEventDefinition();
                    break;

                case "compensateEventDefinition":
                    eventDefinition = new CompensateEventDefinition()
                    {
                        ActivityRef = element.GetAttribute("activityRef"),
                        WaitForCompletion = element.GetBoolean("waitForCompletion")
                    };
                    break;

                case "signalEventDefinition":
                    eventDefinition = new SignalEventDefinition()
                    {
                        SignalRef = element.GetAttribute("signalRef")
                    };
                    break;

                case "escalationEventDefinition":
                    eventDefinition = new EscalationEventDefinition()
                    {
                        EscalationRef = element.GetAttribute("escalationRef")
                    };
                    break;

                case "linkEventDefinition":
                    eventDefinition = new LinkEventDefinition()
                    {
                        Name = element.GetAttribute("name"),
                        Target = element.GetAttribute("target")
                    };
                    break;
            }

            return eventDefinition;
        }
    }
}
