using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    class IOSpecificationParseHandler<TParent> : BaseElementParseHandler<TParent>
    {
        private readonly Action<TParent, InputOutputSpecification> callback;

        public IOSpecificationParseHandler(Action<TParent, InputOutputSpecification> callback)
        {
            this.handlers.Add("dataInput", new DataInputParseHandler<InputOutputSpecification>(
                (io, context, element, result) =>
                {
                    io.DataInputs.Add(result);
                }));

            this.handlers.Add("dataOutput", new DataOutputParseHandler<InputOutputSpecification>(
                (io, context, element, result) =>
                {
                    io.DataOutputs.Add(result);
                }));

            this.handlers.Add("inputSet", new InputSetParseHandler());
            this.handlers.Add("outputSet", new OutputSetParseHandler());

            this.callback = callback;
        }

        public override object Create(TParent parent, IParseContext context, XElement element)
        {
            var io = context.BpmnFactory.CreateIOSpecification();
            if (this.callback != null)
                this.callback(parent, io);

            base.Init(io, context, element);

            return io;
        }
    }

    class DataInputParseHandler<TParent> : BaseElementParseHandler<TParent>
    {
        private readonly Action<TParent, IParseContext, XElement, DataInput> callback;

        public DataInputParseHandler(Action<TParent, IParseContext, XElement, DataInput> callback)
        {
            this.callback = callback;
        }

        public override object Create(TParent parent, IParseContext context, XElement element)
        {
            var dataInput = context.BpmnFactory.CreateDataInput();

            dataInput.Name = element.GetAttribute("name");
            dataInput.IsCollection = element.GetBoolean("isCollection");

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, x => dataInput.ItemSubjectRef = x);

            this.callback(parent, context, element, dataInput);

            base.Init(dataInput, context, element);

            context.Push(dataInput);

            return dataInput;
        }
    }

    class DataOutputParseHandler<TParent> : BaseElementParseHandler<TParent>
    {
        private readonly Action<TParent, IParseContext, XElement, DataOutput> callback;

        public DataOutputParseHandler(Action<TParent, IParseContext, XElement, DataOutput> callback)
        {
            this.callback = callback;
        }

        public override object Create(TParent parent, IParseContext context, XElement element)
        {
            var dataOutput = context.BpmnFactory.CreateDataOutput();

            dataOutput.Name = element.GetAttribute("name");
            dataOutput.IsCollection = element.GetBoolean("isCollection");

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, value => dataOutput.ItemSubjectRef = value);

            if (this.callback != null)
                this.callback(parent, context, element, dataOutput);

            base.Init(dataOutput, context, element);

            context.Push(dataOutput);

            return dataOutput;
        }
    }

    class InputSetParseHandler : BaseElementParseHandler<InputOutputSpecification>
    {
        public InputSetParseHandler()
        {
            this.handlers.Add("dataInputRefs", new ParseHandlerAction<InputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataInput>(value, r => p.DataInputRefs.Add(r));
            }));

            this.handlers.Add("optionalInputRefs", new ParseHandlerAction<InputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataInput>(value, r => p.OptionalInputRefs.Add(r));
            }));

            this.handlers.Add("whileExecutingInputRefs", new ParseHandlerAction<InputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataInput>(value, r => p.WhileExecutingInputRefs.Add(r));
            }));

            this.handlers.Add("outputSetRefs", new ParseHandlerAction<InputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<OutputSet>(value, r => p.OutputSetRefs.Add(r));
            }));
        }

        public override object Create(InputOutputSpecification parent, IParseContext context, XElement element)
        {
            var inputSet = context.BpmnFactory.CreateInputSet();
            parent.InputSets.Add(inputSet);

            inputSet.Name = element.GetAttribute("name");

            base.Init(inputSet, context, element);

            context.Push(inputSet);

            return inputSet;
        }
    }

    class OutputSetParseHandler : BaseElementParseHandler<InputOutputSpecification>
    {
        public OutputSetParseHandler()
        {
            this.handlers.Add("dataOutputRefs", new ParseHandlerAction<OutputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataOutput>(value, r => p.DataOutputRefs.Add(r));
            }));

            this.handlers.Add("optionalOutputRefs", new ParseHandlerAction<OutputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataOutput>(value, r => p.OptionalOutputRefs.Add(r));
            }));

            this.handlers.Add("whileExecutingOutputRefs", new ParseHandlerAction<OutputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<DataOutput>(value, r => p.WhileExecutingOutputRefs.Add(r));
            }));

            this.handlers.Add("inputSetRefs", new ParseHandlerAction<OutputSet>((p, c, x) =>
            {
                var value = x.Value;
                if (value != null)
                    c.AddReferenceRequest<InputSet>(value, r => p.InputSetRefs.Add(r));
            }));
        }

        public override object Create(InputOutputSpecification parent, IParseContext context, XElement element)
        {
            var outputSet = context.BpmnFactory.CreateOutputSet();
            parent.OutputSets.Add(outputSet);

            outputSet.Name = element.GetAttribute("name");

            base.Init(outputSet, context, element);

            context.Push(outputSet);

            return outputSet;
        }
    }
}
