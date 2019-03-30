using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    abstract class ActivityParseHandler : FlowNodeParseHandler
    {
        public ActivityParseHandler()
        {
            this.handlers.Add("property", new PropertyParseHandler());
            this.handlers.Add("ioSpecification", new IOSpecificationParseHandler());

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

            //this.handlers.Add("dataInputAssociation", new DataInputAssociationHandler<TActivity>());
            //this.handlers.Add("dataOutputAssociation", new DataOutputAssociationHandler<TActivity>());
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

            this.handlers.Add("inputDataItem", new DataInputParseHandler());

            this.handlers.Add("outputDataItem", new DataOutputParseHandler());

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

            item.IsSequential = element.GetBoolean("isSequential");

            var eventRef = element.GetAttribute("noneBehaviorEventRef");
            context.AddReferenceRequest(eventRef, (EventDefinition target) => item.NoneBehaviorEventRef = target);

            eventRef = element.GetAttribute("oneBehaviorEventRef");
            context.AddReferenceRequest(eventRef, (EventDefinition target) => item.OneBehaviorEventRef = target);

            item.Behavior = element.GetEnum("behavior", MultiInstanceBehavior.None);

            //extended attributes.
            item.CollectionRef = element.GetExtendedAttribute("collectionRef");
            item.ElementRef = element.GetExtendedAttribute("elementRef");

            parent.LoopCharacteristics = item;

            return item;
        }

        class DataInputParseHandler : BaseElementParseHandler<MultiInstanceLoopCharacteristics>
        {
            public override object Create(MultiInstanceLoopCharacteristics parent, IParseContext context, XElement element)
            {
                var dataInput = context.BpmnFactory.CreateDataInput();

                dataInput.Name = element.GetAttribute("name");
                //dataInput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
                dataInput.IsCollection = element.GetBoolean("isCollection");

                parent.InputDataItem = dataInput;

                base.Init(dataInput, context, element);

                return dataInput;
            } 
        }

        class DataOutputParseHandler: BaseElementParseHandler<MultiInstanceLoopCharacteristics>
        {
            //private readonly Action<TParent, IParseContext, XElement, DataOutput> callback;

            //public DataOutputHandler(Action<TParent, IParseContext, XElement, DataOutput> callback)
            //{
            //    this.callback = callback;
            //}

            public override object Create(MultiInstanceLoopCharacteristics parent, IParseContext context, XElement element)
            {
                var dataOutput = context.BpmnFactory.CreateDataOutput();

                dataOutput.Name = element.GetAttribute("name");
                //dataOutput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
                dataOutput.IsCollection = element.GetBoolean("isCollection");

                parent.OutputDataItem = dataOutput;

                base.Init(dataOutput, context, element);

                return dataOutput;
            }
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

            item.TestBefore = element.GetBoolean("testBefore");

            var value = element.GetAttribute("loopMaximum");
            if (value != null)
                item.LoopMaximum = Convert.ToInt32(value);

            parent.LoopCharacteristics = item;

            base.Init(item, context, element);

            return item;
        }
    }
}
