using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    class EventDefinitionParseHandler : BaseElementParseHandler<Definitions>
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

        public EventDefinitionParseHandler()
        {
            //this.handlers.Add("source", new BpmnHandlerCallback<EventDefinition>((p, c, x) =>
            //{
            //    var source = x.Value;
            //    var linkEvent = ((LinkEventDefinition)p);
            //    linkEvent.Source.Add(source);

            //    return source;
            //}));

            this.handlers.Add("condition", new ExpressionParseHandler<EventDefinition>((p, expr) =>
            {
                var conditional = ((ConditionalEventDefinition)p);
                conditional.Condition = expr;
            }));

            this.handlers.Add("timeDuration", new ExpressionParseHandler<EventDefinition>((p, expr) =>
            {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeDuration = expr;
            }));

            this.handlers.Add("timeDate", new ExpressionParseHandler<EventDefinition>((p, expr) =>
            {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeDate = expr;
            }));

            this.handlers.Add("timeCycle", new ExpressionParseHandler<EventDefinition>((p, expr) =>
            {
                var timerEvent = ((TimerEventDefinition)p);
                timerEvent.TimeCycle = expr;
            }));
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            EventDefinition eventDefinition = null;
            var localName = Helper.GetRealLocalName(element);

            switch (localName)
            {
                case "cancelEventDefinition":
                    eventDefinition = new CancelEventDefinition();
                    break;

                case "errorEventDefinition":
                    eventDefinition = new ErrorEventDefinition();
                    var errorRef = element.GetAttribute("errorRef");
                    if (errorRef != null)
                        context.AddReferenceRequest(errorRef, (Error error) => ((ErrorEventDefinition)eventDefinition).ErrorRef = error);
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
                        //OperationRef = element.GetAttribute("operationRef"),
                        //MessageRef = element.GetAttribute("messageRef")
                    };
                    break;

                case "conditionalEventDefinition":
                    eventDefinition = new ConditionalEventDefinition();
                    break;

                case "compensateEventDefinition":
                    eventDefinition = new CompensateEventDefinition()
                    {
                        //ActivityRef = element.GetAttribute("activityRef"),
                        WaitForCompletion = element.GetBoolean("waitForCompletion")
                    };
                    break;

                case "signalEventDefinition":
                    eventDefinition = new SignalEventDefinition()
                    {
                        //SignalRef = element.GetAttribute("signalRef")
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
