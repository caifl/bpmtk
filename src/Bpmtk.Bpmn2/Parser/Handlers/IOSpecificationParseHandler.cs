using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class IOSpecificationParseHandler<TParent> : BaseElementParseHandler<TParent>
    {
        private readonly Action<TParent, IOSpecification> callback;

        public IOSpecificationParseHandler(Action<TParent, IOSpecification> callback)
        {
            this.handlers.Add("dataInput", new DataInputParseHandler<IOSpecification>(
                (io, context, element, result) =>
                {
                    io.DataInputs.Add(result);
                }));

            this.handlers.Add("dataOutput", new DataOutputParseHandler<IOSpecification>(
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

            return dataOutput;
        }
    }

    class InputSetParseHandler : BaseElementParseHandler<IOSpecification>
    {
        public InputSetParseHandler()
        {
            //this.handlers.Add("dataInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.DataInputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("optionalInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.OptionalInputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("whileExecutingInputRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.WhileExecutingInputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("outputSetRefs", new BpmnHandlerCallback<InputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.OutputSetRefs.Add(value);

            //    return value;
            //}));
        }

        public override object Create(IOSpecification parent, IParseContext context, XElement element)
        {
            var inputSet = context.BpmnFactory.CreateInputSet();
            parent.InputSets.Add(inputSet);

            inputSet.Name = element.GetAttribute("name");

            base.Init(inputSet, context, element);

            return inputSet;
        }
    }

    class OutputSetParseHandler : BaseElementParseHandler<IOSpecification>
    {
        public OutputSetParseHandler()
        {
            //this.handlers.Add("dataOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.DataOutputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("optionalOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.OptionalOutputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("whileExecutingOutputRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.WhileExecutingOutputRefs.Add(value);

            //    return value;
            //}));

            //this.handlers.Add("inputSetRefs", new BpmnHandlerCallback<OutputSet>((p, c, x) =>
            //{
            //    var value = x.Value;

            //    p.InputSetRefs.Add(value);

            //    return value;
            //}));
        }

        public override object Create(IOSpecification parent, IParseContext context, XElement element)
        {
            var outputSet = context.BpmnFactory.CreateOutputSet();
            parent.OutputSets.Add(outputSet);

            outputSet.Name = element.GetAttribute("name");

            base.Init(outputSet, context, element);

            return outputSet;
        }
    }
}
