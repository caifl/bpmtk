using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ProcessHandler : BaseElementHandler<Definitions, Process>
    {
        public ProcessHandler()
        {
            this.handlers.Add("startEvent", new StartEventHandler<Process>());
            this.handlers.Add("userTask", new UserTaskHandler<Process>());
            this.handlers.Add("sequenceFlow", new SequenceFlowHandler<Process>());
            this.handlers.Add("endEvent", new EndEventHandler<Process>());
            this.handlers.Add("parallelGateway", new ParallelGatewayHandler<Process>());
            this.handlers.Add("exclusiveGateway", new ExclusiveGatewayHandler<Process>());
            this.handlers.Add("inclusiveGateway", new InclusiveGatewayHandler<Process>());
            this.handlers.Add("scriptTask", new ScriptTaskHandler<Process>());
            this.handlers.Add("serviceTask", new ServiceTaskHandler<Process>());
            this.handlers.Add("receiveTask", new ReceiveTaskHandler<Process>());
            this.handlers.Add("sendTask", new SendTaskHandler<Process>());
            this.handlers.Add("task", new TaskHandler<Process>());
            this.handlers.Add("manualTask", new ManualTaskHandler<Process>());
            this.handlers.Add("businessRuleTask", new BusinessRuleTaskHandler<Process>());

            this.handlers.Add("subProcess", new SubProcessHandler<Process>());
            this.handlers.Add("transaction", new TransactionHandler<Process>());
            this.handlers.Add("adHocSubProcess", new AdHocSubProcessHandler<Process>());

            this.handlers.Add("property", new PropertyHandler<Process>());
            this.handlers.Add("dataObject", new DataObjectHandler<Process>());

            this.handlers.Add("potentialOwner", new ResourceRoleHandler<Process>());
            this.handlers.Add("humanPerformer", new ResourceRoleHandler<Process>());
            this.handlers.Add("performer", new ResourceRoleHandler<Process>());
            this.handlers.Add("resourceRole", new ResourceRoleHandler<Process>());

            this.handlers.Add("ioBinding", new InputOutputBindingHandler<Process>());
            this.handlers.Add("ioSpecification", new IOSpecificationHandler<Process>());

            this.handlers.Add("dataObjectReference", new DataObjectReferenceHandler<Process>());

            var artifactHandler = new ArtifactHandler<Process>();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        public override Process Create(Definitions parent, IParseContext context, XElement element)
        {
            var process = base.Create(parent, context, element);

            process.Name = element.GetAttribute("name");
            process.IsExecutable = element.GetBoolean("isExecutable", false);
            process.ProcessType = element.GetEnum("processType", ProcessType.None);
            process.IsClosed = element.GetBoolean("isClosed", false);

            parent.RootElements.Add(process);

            var scope = context.PopScope();
            scope.Complete();

            if (process.ExtensionElements != null)
            {
                var items = this.ParseEventListeners(process.ExtensionElements);
                foreach (var item in items)
                    process.EventListeners.Add(item);
            }

            return process;
        }

        protected override Process New(IParseContext context, XElement element)
        {
            var process = context.BpmnFactory.CreateProcess();

            var scope = new FlowElementScope(process);
            context.PushScope(scope);

            return process;
        }
    }

    class InputOutputBindingHandler<TParent> : BaseElementHandler<TParent, InputOutputBinding>
        where TParent : CallableElement
    {
        public override InputOutputBinding Create(TParent parent, IParseContext context, XElement element)
        {
            var ioBinding = base.Create(parent, context, element);

            ioBinding.OutputDataRef = element.GetAttribute("outputDataRef");
            ioBinding.InputDataRef = element.GetAttribute("inputDataRef");
            ioBinding.OperationRef = element.GetAttribute("operationRef");

            parent.IOBindings.Add(ioBinding);

            return ioBinding;
        }

        protected override InputOutputBinding New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateInputOutputBinding();
    }
}