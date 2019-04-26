using Bpmtk.Bpmn2;
using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class EventDefinitionParseHandler : BaseElementParseHandler
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
        private readonly Action<object, IParseContext, XElement, EventDefinition> callback;

        public EventDefinitionParseHandler(Action<object, IParseContext, XElement, EventDefinition> callback)
        {
            this.handlers.Add("source", new ParseHandlerAction<EventDefinition>((p, c, x) =>
            {
                var source = x.Value;
                var linkEvent = ((LinkEventDefinition)p);
                linkEvent.Source.Add(source);
            }));

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
            this.callback = callback;
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            EventDefinition eventDefinition = null;
            var localName = Helper.GetRealLocalName(element);

            switch (localName)
            {
                case "cancelEventDefinition":
                    eventDefinition = new CancelEventDefinition();
                    break;

                case "errorEventDefinition":
                    var errorEventDefinition = new ErrorEventDefinition();
                    eventDefinition = errorEventDefinition;
                    var errorRef = element.GetAttribute("errorRef");
                    if (errorRef != null)
                        context.AddReferenceRequest<Error>(errorRef, x => errorEventDefinition.ErrorRef = x);
                    break;

                case "timerEventDefinition":
                    eventDefinition = new TimerEventDefinition();
                    break;

                case "terminateEventDefinition":
                    eventDefinition = new TerminateEventDefinition();
                    break;

                case "messageEventDefinition":
                    var messageEventDefinition = new MessageEventDefinition();
                    eventDefinition = messageEventDefinition;

                    var operationRef = element.GetAttribute("operationRef");
                    if (operationRef != null)
                        context.AddReferenceRequest<Operation>(operationRef, x => messageEventDefinition.OperationRef = x);

                    var messageRef = element.GetAttribute("messageRef");
                    if (messageRef != null)
                        context.AddReferenceRequest<Message>(messageRef, x => messageEventDefinition.MessageRef = x);
                    break;

                case "conditionalEventDefinition":
                    eventDefinition = new ConditionalEventDefinition();
                    break;

                case "compensateEventDefinition":
                    var compensateEventDefinition = new CompensateEventDefinition();
                    eventDefinition = compensateEventDefinition;

                    compensateEventDefinition.WaitForCompletion = element.GetBoolean("waitForCompletion");
                    var activityRef = element.GetAttribute("activityRef");
                    if (activityRef != null)
                        context.AddReferenceRequest<Activity>(activityRef, x => compensateEventDefinition.ActivityRef = x);
                    break;

                case "signalEventDefinition":
                    var signalEventDefinition = new SignalEventDefinition();
                    eventDefinition = signalEventDefinition;

                    var signalRef = element.GetAttribute("signalRef");
                    if (signalRef != null)
                        context.AddReferenceRequest<Signal>(signalRef, x => signalEventDefinition.SignalRef = x);
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

            if (this.callback != null)
                this.callback(parent, context, element, eventDefinition);

            base.Init(eventDefinition, context, element);

            context.Push(eventDefinition);

            return eventDefinition;
        }
    }
}
