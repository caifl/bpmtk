using Bpmtk.Bpmn2;
using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class ProcessParseHandler : CallableElementParseHandler
    {
        public ProcessParseHandler()
        {
            this.handlers.Add("dataObject", new DataObjectParseHandler());
            this.handlers.Add("dataObjectReference", new DataObjectReferenceParseHandler());

            this.handlers.Add("property", new PropertyParseHandler<Process>((proc, props) =>
            {
                proc.Properties.Add(props);
            }));

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
            this.handlers.Add("complexGateway", new ComplexGatewayParseHandler());
            this.handlers.Add("scriptTask", new ScriptTaskParseHandler());
            this.handlers.Add("serviceTask", new ServiceTaskParseHandler());
            this.handlers.Add("receiveTask", new ReceiveTaskParseHandler());
            this.handlers.Add("sendTask", new SendTaskParseHandler());
            this.handlers.Add("task", new TaskParseHandler());
            this.handlers.Add("manualTask", new ManualTaskParseHandler());
            this.handlers.Add("businessRuleTask", new BusinessRuleTaskParseHandler());
            this.handlers.Add("callActivity", new CallActivityParseHandler());

            this.handlers.Add("subProcess", new SubProcessParseHandler());
            this.handlers.Add("transaction",new TransactionParseHandler());
            this.handlers.Add("adHocSubProcess", new AdHocSubProcessParseHandler());

            var artifactHandler = new ArtifactParseHandler();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var process = context.BpmnFactory.CreateProcess();
            parent.RootElements.Add(process);

            process.IsExecutable = element.GetBoolean("isExecutable", false);
            process.ProcessType = element.GetEnum("processType", ProcessType.None);
            process.IsClosed = element.GetBoolean("isClosed", false);

            base.Init(process, context, element);

            return process;
        }
    }
}
