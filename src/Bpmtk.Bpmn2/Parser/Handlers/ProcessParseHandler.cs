using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    class ProcessParseHandler : BaseElementParseHandler<Definitions>
    {
        public ProcessParseHandler()
        {
            this.handlers.Add("dataObject", new DataObjectParseHandler());
            this.handlers.Add("dataObjectReference", new DataObjectReferenceParseHandler());
            this.handlers.Add("property", new PropertyParseHandler());
            this.handlers.Add("ioBinding", new InputOutputBindingParseHandler());
            this.handlers.Add("ioSpecification", new IOSpecificationParseHandler());

            //resource
            IParseHandler handler = new ResourceRoleParseHandler();
            foreach (var item in ResourceRoleParseHandler.Keys)
                this.handlers.Add(item, handler);

            this.handlers.Add("startEvent", new StartEventParseHandler());
            this.handlers.Add("userTask", new UserTaskParseHandler());
            this.handlers.Add("sequenceFlow", new SequenceFlowParseHandler());
            this.handlers.Add("endEvent", new EndEventParseHandler());
            this.handlers.Add("parallelGateway", new ParallelGatewayParseHandler());
            this.handlers.Add("exclusiveGateway", new ExclusiveGatewayParseHandler());
            this.handlers.Add("inclusiveGateway", new InclusiveGatewayParseHandler());
            this.handlers.Add("scriptTask", new ScriptTaskParseHandler());
            this.handlers.Add("serviceTask", new ServiceTaskParseHandler());
            this.handlers.Add("receiveTask", new ReceiveTaskParseHandler());
            this.handlers.Add("sendTask", new SendTaskParseHandler());
            this.handlers.Add("task", new TaskParseHandler());
            this.handlers.Add("manualTask", new ManualTaskParseHandler());
            this.handlers.Add("businessRuleTask", new BusinessRuleTaskParseHandler());

            this.handlers.Add("subProcess", new SubProcessParseHandler());
            //this.handlers.Add("transaction", new TransactionHandler<Process>());
            //this.handlers.Add("adHocSubProcess", new AdHocSubProcessHandler<Process>());

            var artifactHandler = new ArtifactParseHandler();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var process = context.BpmnFactory.CreateProcess();

            process.Name = element.GetAttribute("name");
            process.IsExecutable = element.GetBoolean("isExecutable", false);
            process.ProcessType = element.GetEnum("processType", ProcessType.None);
            process.IsClosed = element.GetBoolean("isClosed", false);

            base.Init(process, context, element);

            parent.RootElements.Add(process);

            

            return process;
        }

        class InputOutputBindingParseHandler : BaseElementParseHandler
        {
            public override object Create(object parent, IParseContext context, XElement element)
            {
                var process = parent as Process;

                var ioBinding = context.BpmnFactory.CreateInputOutputBinding();

                //ioBinding.OutputDataRef = element.GetAttribute("outputDataRef");
                //ioBinding.InputDataRef = element.GetAttribute("inputDataRef");
                //ioBinding.OperationRef = element.GetAttribute("operationRef");

                process.IOBindings.Add(ioBinding);

                return ioBinding;
            }
        }
    }
}
