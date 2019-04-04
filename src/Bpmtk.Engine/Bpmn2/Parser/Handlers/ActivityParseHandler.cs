using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    abstract class ActivityParseHandler : FlowNodeParseHandler
    {
        public ActivityParseHandler()
        {
            this.handlers.Add("property", new PropertyParseHandler<Activity>((act,props) =>
            {
                act.Properties.Add(props);
            }));

            this.handlers.Add("ioSpecification", new IOSpecificationParseHandler<Activity>((x, y) =>
            {
                x.IOSpecification = y;
            }));

            var miHandler = new MultiInstanceLoopCharacteristicsHandler();
            var stdHandler = new StandardLoopCharacteristicsHandler();

            this.handlers.Add("loopCharacteristics", new ParseHandlerAction<Activity>((p, c, x) =>
            {
                var localName = Helper.GetRealLocalName(x);
                if (localName == "multiInstanceLoopCharacteristics")
                    p.LoopCharacteristics = (LoopCharacteristics)miHandler.Create(p, c, x);
                else
                    p.LoopCharacteristics = (LoopCharacteristics)stdHandler.Create(p, c, x);
            }));

            this.handlers.Add("multiInstanceLoopCharacteristics", miHandler);
            this.handlers.Add("standardLoopCharacteristics", stdHandler);

            var handler = new ResourceRoleParseHandler();
            foreach (var key in ResourceRoleParseHandler.Keys)
                this.handlers.Add(key, handler);

            this.handlers.Add("dataInputAssociation", new DataInputAssociationParseHandler<Activity>((x, y) =>
            {
                x.DataInputAssociations.Add(y);
            }));

            this.handlers.Add("dataOutputAssociation", new DataOutputAssociationParseHandler<Activity>((x, y) =>
            {
                x.DataOutputAssociations.Add(y);
            }));
        }

        protected virtual void Init(Activity activity, IParseContext context, XElement element)
        {
            activity.IsForCompensation = element.GetBoolean("isForCompensation");
            activity.StartQuantity = element.GetInt32("startQuantity");
            activity.CompletionQuantity = element.GetInt32("completionQuantity");

            var defaultOutgoing = element.GetAttribute("default");
            if (defaultOutgoing != null)
                context.AddReferenceRequest<SequenceFlow>(defaultOutgoing, (x) => activity.Default = x);

            base.Init(activity, context, element);
        }
    }

    class MultiInstanceLoopCharacteristicsHandler : BaseElementParseHandler<Activity>
    {
        public MultiInstanceLoopCharacteristicsHandler()
        {
            this.handlers.Add("loopCardinality", new ExpressionParseHandler<MultiInstanceLoopCharacteristics>((p, expr) =>
            {
                p.LoopCardinality = expr;
            }));

            this.handlers.Add("completionCondition", new ExpressionParseHandler<MultiInstanceLoopCharacteristics>((p, expr) =>
            {
                p.CompletionCondition = expr;
            }));

            this.handlers.Add("inputDataItem", new DataInputParseHandler<MultiInstanceLoopCharacteristics>(
                (target, context, element, result) =>
                {
                    target.InputDataItem = result;
                }));

            this.handlers.Add("outputDataItem", new DataOutputParseHandler<MultiInstanceLoopCharacteristics>(
                (target, context, element, result) =>
                {
                    target.OutputDataItem = result;
                }));

            this.handlers.Add("loopDataInputRef", new ParseHandlerAction<MultiInstanceLoopCharacteristics>((p, c, x) =>
            {
                var loopDataInputRef = x.Value;
                if(loopDataInputRef != null)
                    c.AddReferenceRequest<IItemAwareElement>(loopDataInputRef, (r) => p.LoopDataInputRef = r);
            }));

            this.handlers.Add("loopDataOutputRef", new ParseHandlerAction<MultiInstanceLoopCharacteristics>((p, c, x) =>
            {
                var loopDataOutputRef = x.Value;
                if (loopDataOutputRef != null)
                    c.AddReferenceRequest<IItemAwareElement>(loopDataOutputRef, (r) => p.LoopDataOutputRef = r);
            }));
        }

        public override object Create(Activity parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateMultiInstanceLoopCharacteristics();
            parent.LoopCharacteristics = item;
            item.Activity = parent;

            item.IsSequential = element.GetBoolean("isSequential");

            var eventRef = element.GetAttribute("noneBehaviorEventRef");
            context.AddReferenceRequest<EventDefinition>(eventRef, x => item.NoneBehaviorEventRef = x);

            eventRef = element.GetAttribute("oneBehaviorEventRef");
            context.AddReferenceRequest<EventDefinition>(eventRef, x => item.OneBehaviorEventRef = x);

            item.Behavior = element.GetEnum("behavior", MultiInstanceBehavior.None);

            //extended attributes.
            //item.CollectionRef = element.GetExtendedAttribute("collectionRef");
            //item.ElementRef = element.GetExtendedAttribute("elementRef");

            return item;
        }
    }

    class StandardLoopCharacteristicsHandler : BaseElementParseHandler<Activity>
    {
        public StandardLoopCharacteristicsHandler()
        {
            this.handlers.Add("loopCondition", new ExpressionParseHandler<StandardLoopCharacteristics>((p, expr) =>
            {
                p.LoopCondition = expr;
            }));
        }

        public override object Create(Activity parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateStandardLoopCharacteristics();
            parent.LoopCharacteristics = item;
            item.Activity = parent;

            item.TestBefore = element.GetBoolean("testBefore");

            var value = element.GetAttribute("loopMaximum");
            if (value != null)
                item.LoopMaximum = Convert.ToInt32(value);

            base.Init(item, context, element);

            return item;
        }
    }
}
