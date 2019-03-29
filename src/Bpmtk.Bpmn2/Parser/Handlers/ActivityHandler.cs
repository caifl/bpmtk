using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class ActivityHandler<TFlowElementContainer, TActivity> : FlowNodeHandler<TFlowElementContainer, TActivity>
        where TActivity : Activity
        where TFlowElementContainer : IFlowElementsContainer
    {
        public ActivityHandler()
        {
            this.handlers.Add("property", new PropertyHandler<TActivity>());
            this.handlers.Add("ioSpecification", new IOSpecificationHandler<TActivity>());

            var miHandler = new MultiInstanceLoopCharacteristicsHandler<TActivity>();
            var stdHandler = new StandardLoopCharacteristicsHandler<TActivity>();

            this.handlers.Add("loopCharacteristics", new BpmnHandlerCallback<TActivity>((p, c, x) =>
            {
                var localName = Helper.GetRealLocalName(x);
                if (localName == "multiInstanceLoopCharacteristics")
                    return miHandler.Create(p, c, x);

                return stdHandler.Create(p, c,x);
            }));

            this.handlers.Add("multiInstanceLoopCharacteristics", miHandler);
            this.handlers.Add("standardLoopCharacteristics", stdHandler);

            var resourceRoleHandler = new ResourceRoleHandler<TActivity>();
            foreach (var key in ResourceRoleHandler<TActivity>.Keys)
            {
                this.handlers.Add(key, resourceRoleHandler);
            }

            this.handlers.Add("dataInputAssociation", new DataInputAssociationHandler<TActivity>());
            this.handlers.Add("dataOutputAssociation", new DataOutputAssociationHandler<TActivity>());
        }

        public override TActivity Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var activity = base.Create(parent, context, element);

            activity.IsForCompensation = element.GetBoolean("isForCompensation");
            activity.StartQuantity = element.GetInt32("startQuantity");
            activity.CompletionQuantity = element.GetInt32("completionQuantity");

            var defaultOutgoing = element.GetAttribute("default");
            if (defaultOutgoing != null)
            {
                var scope = context.PeekScope();
                scope.AddDefault(defaultOutgoing, activity);
            }

            return activity;
        }
    }

    class MultiInstanceLoopCharacteristicsHandler<TActivity> : BaseElementHandler<TActivity, MultiInstanceLoopCharacteristics>
        where TActivity : Activity
    {
        public MultiInstanceLoopCharacteristicsHandler()
        {
            this.handlers.Add("loopCardinality", new ExpressionHandler<MultiInstanceLoopCharacteristics>((p, c, x, expr) =>
            {
                p.LoopCardinality = expr;
            }));

            this.handlers.Add("completionCondition", new ExpressionHandler<MultiInstanceLoopCharacteristics>((p, c, x, expr) =>
            {
                p.CompletionCondition = expr;
            }));

            this.handlers.Add("inputDataItem", new DataInputHandler());

            this.handlers.Add("outputDataItem", new DataOutputHandler());

            this.handlers.Add("loopDataInputRef", new BpmnHandlerCallback<MultiInstanceLoopCharacteristics>((p, c, x) =>
            {
                return p.LoopDataInputRef = x.Value;
            }));

            this.handlers.Add("loopDataOutputRef", new BpmnHandlerCallback<MultiInstanceLoopCharacteristics>((p, c, x) =>
            {
                return p.LoopDataOutputRef = x.Value;
            }));
        }

        public override MultiInstanceLoopCharacteristics Create(TActivity parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);

            item.IsSequential = element.GetBoolean("isSequential");
            item.NoneBehaviorEventRef = element.GetAttribute("noneBehaviorEventRef");
            item.OneBehaviorEventRef = element.GetAttribute("oneBehaviorEventRef");
            item.Behavior = element.GetEnum("behavior", MultiInstanceBehavior.None);

            //extended attributes.
            item.CollectionRef = element.GetExtendedAttribute("collectionRef");
            item.ElementRef = element.GetExtendedAttribute("elementRef");

            parent.LoopCharacteristics = item;

            return item;
        }

        protected override MultiInstanceLoopCharacteristics New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateMultiInstanceLoopCharacteristics();

        class DataInputHandler : BaseElementHandler<MultiInstanceLoopCharacteristics, DataInput>
        {
            public override DataInput Create(MultiInstanceLoopCharacteristics parent, IParseContext context, XElement element)
            {
                var dataInput = base.Create(parent, context, element);

                dataInput.Name = element.GetAttribute("name");
                dataInput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
                dataInput.IsCollection = element.GetBoolean("isCollection");

                parent.InputDataItem = dataInput;

                return dataInput;
            }

            protected override DataInput New(IParseContext context, XElement element)
                => context.BpmnFactory.CreateDataInput();
        }

        class DataOutputHandler: BaseElementHandler<MultiInstanceLoopCharacteristics, DataOutput>
        {
            //private readonly Action<TParent, IParseContext, XElement, DataOutput> callback;

            //public DataOutputHandler(Action<TParent, IParseContext, XElement, DataOutput> callback)
            //{
            //    this.callback = callback;
            //}

            public override DataOutput Create(MultiInstanceLoopCharacteristics parent, IParseContext context, XElement element)
            {
                var dataOutput = base.Create(parent, context, element);

                dataOutput.Name = element.GetAttribute("name");
                dataOutput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
                dataOutput.IsCollection = element.GetBoolean("isCollection");

                parent.OutputDataItem = dataOutput;

                return dataOutput;
            }

            protected override DataOutput New(IParseContext context, XElement element)
                => context.BpmnFactory.CreateDataOutput();
        }
    }

    class StandardLoopCharacteristicsHandler<TActivity> : BaseElementHandler<TActivity, StandardLoopCharacteristics>
        where TActivity : Activity
    {
        public StandardLoopCharacteristicsHandler()
        {
            this.handlers.Add("loopCondition", new ExpressionHandler<StandardLoopCharacteristics>((p, c, x, expr) =>
            {
                p.LoopCondition = expr;
            }));
        }

        public override StandardLoopCharacteristics Create(TActivity parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);

            item.TestBefore = element.GetBoolean("testBefore");

            var value = element.GetAttribute("loopMaximum");
            if (value != null)
                item.LoopMaximum = Convert.ToInt32(value);

            parent.LoopCharacteristics = item;

            return item;
        }

        protected override StandardLoopCharacteristics New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateStandardLoopCharacteristics();
    }
}
