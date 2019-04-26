using Bpmtk.Bpmn2;
using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    abstract class CallableElementParseHandler : BaseElementParseHandler<Definitions>
    {
        public CallableElementParseHandler()
        {
            this.handlers.Add("ioBinding", new InputOutputBindingParseHandler());
            this.handlers.Add("ioSpecification", new IOSpecificationParseHandler<Process>((x, y) =>
            {
                x.IOSpecification = y;
            }));

            this.handlers.Add("supportedInterfaceRef", new ParseHandlerAction<CallableElement>((p, c, x) =>
            {
                var id = x.Value;
                if (id != null)
                    c.AddReferenceRequest<Interface>(id, r => p.SupportedInterfaceRefs.Add(r));
            }));
        }

        protected virtual void Init(CallableElement callableElement, IParseContext context, XElement element)
        {
            callableElement.Name = element.GetAttribute("name");

            base.Init(callableElement, context, element);
        }
    }

    class InputOutputBindingParseHandler : BaseElementParseHandler<CallableElement>
    {
        public override object Create(CallableElement parent, IParseContext context, XElement element)
        {
            var ioBinding = context.BpmnFactory.CreateInputOutputBinding();
            parent.IOBindings.Add(ioBinding);

            var outputDataRef = element.GetAttribute("outputDataRef");
            if (outputDataRef != null)
                context.AddReferenceRequest<OutputSet>(outputDataRef, r => ioBinding.OutputDataRef = r);

            var inputDataRef = element.GetAttribute("inputDataRef");
            if (inputDataRef != null)
                context.AddReferenceRequest<InputSet>(inputDataRef, r => ioBinding.InputDataRef = r);

            var operationRef = element.GetAttribute("operationRef");
            if (operationRef != null)
                context.AddReferenceRequest<Operation>(operationRef, x => ioBinding.OperationRef = x);

            base.Init(ioBinding, context, element);
            
            return ioBinding;
        }
    }
}
