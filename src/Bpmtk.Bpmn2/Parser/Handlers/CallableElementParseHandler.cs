using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
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

            //ioBinding.OutputDataRef = element.GetAttribute("outputDataRef");
            //ioBinding.InputDataRef = element.GetAttribute("inputDataRef");

            var operationRef = element.GetAttribute("operationRef");
            if (operationRef != null)
                context.AddReferenceRequest<Operation>(operationRef, x => ioBinding.OperationRef = x);

            base.Init(ioBinding, context, element);
            
            return ioBinding;
        }
    }
}
