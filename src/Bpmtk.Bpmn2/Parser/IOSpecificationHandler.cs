using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class IOSpecificationHandler<TParent> : BaseElementHandler<TParent, IOSpecification>
        where TParent : IHasIOSpecification
    {
        //private readonly Action<TParent, IParseContext, XElement, IOSpecification> callback;

        //public IOSpecificationHandler(Action<TParent, IParseContext, XElement, IOSpecification> callback)
        public IOSpecificationHandler()
        {
            this.handlers.Add("dataInput", new DataInputHandler<IOSpecification>());

            this.handlers.Add("dataOutput", new DataOutputHandler<IOSpecification>());

            this.handlers.Add("inputSet", new InputSetHandler());

            this.handlers.Add("outputSet", new OutputSetHandler());

            //this.callback = callback;
        }

        public override IOSpecification Create(TParent parent, IParseContext context, XElement element)
        {
            var io = base.Create(parent, context, element);

            //this.callback(parent, context, element, io);
            parent.IOSpecification = io;

            return io;
        }

        protected override IOSpecification New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateIOSpecification();
    }

    class DataInputHandler<TParent> : BaseElementHandler<TParent, DataInput>
        where TParent : IHasDataInputs
    {
        //private readonly Action<TParent, IParseContext, XElement, DataInput> callback;

        //public DataInputHandler(Action<TParent, IParseContext, XElement, DataInput> callback)
        //{
        //    this.callback = callback;
        //}

        public override DataInput Create(TParent parent, IParseContext context, XElement element)
        {
            var dataInput = base.Create(parent, context, element);

            dataInput.Name = element.GetAttribute("name");
            dataInput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            dataInput.IsCollection = element.GetBoolean("isCollection");

            //this.callback(parent, context, element, dataInput);
            parent.DataInputs.Add(dataInput);

            return dataInput;
        }

        protected override DataInput New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataInput();
    }

    class DataOutputHandler<TParent> : BaseElementHandler<TParent, DataOutput>
        where TParent : IHasDataOutputs
    {
        //private readonly Action<TParent, IParseContext, XElement, DataOutput> callback;

        //public DataOutputHandler(Action<TParent, IParseContext, XElement, DataOutput> callback)
        //{
        //    this.callback = callback;
        //}

        public override DataOutput Create(TParent parent, IParseContext context, XElement element)
        {
            var dataOutput = base.Create(parent, context, element);

            dataOutput.Name = element.GetAttribute("name");
            dataOutput.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            dataOutput.IsCollection = element.GetBoolean("isCollection");

            //this.callback(parent, context, element, dataOutput);
            parent.DataOutputs.Add(dataOutput);

            return dataOutput;
        }

        protected override DataOutput New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataOutput();
    }

    class InputSetHandler : BaseElementHandler<IOSpecification, InputSet>
    {
        public InputSetHandler()
        {
            this.handlers.Add("dataInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.DataInputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("optionalInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.OptionalInputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("whileExecutingInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.WhileExecutingInputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("outputSetRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.OutputSetRefs.Add(value);

                return value;
            }));
        }

        public override InputSet Create(IOSpecification parent, IParseContext context, XElement element)
        {
            var inputSet = base.Create(parent, context, element);

            inputSet.Name = element.GetAttribute("name");

            parent.InputSets.Add(inputSet);

            return inputSet;
        }

        protected override InputSet New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateInputSet();
    }

    class OutputSetHandler : BaseElementHandler<IOSpecification, OutputSet>
    {
        public OutputSetHandler()
        {
            this.handlers.Add("dataOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.DataOutputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("optionalOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.OptionalOutputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("whileExecutingOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.WhileExecutingOutputRefs.Add(value);

                return value;
            }));

            this.handlers.Add("inputSetRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            {
                var value = x.Value;

                p.InputSetRefs.Add(value);

                return value;
            }));
        }

        public override OutputSet Create(IOSpecification parent, IParseContext context, XElement element)
        {
            var outputSet = base.Create(parent, context, element);

            outputSet.Name = element.GetAttribute("name");

            parent.OutputSets.Add(outputSet);

            return outputSet;
        }

        protected override OutputSet New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateOutputSet();
    }
}
