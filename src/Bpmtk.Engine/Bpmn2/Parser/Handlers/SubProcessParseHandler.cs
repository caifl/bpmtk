using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class SubProcessParseHandler : FlowElementParseHandler
    {
        public SubProcessParseHandler()
        {
            this.handlers.Add("dataObject", new DataObjectParseHandler());
            this.handlers.Add("dataObjectReference", new DataObjectReferenceParseHandler());

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

            this.handlers.Add("subProcess", ProcessParseHandler.SubProcessParseHandler);
            this.handlers.Add("transaction", ProcessParseHandler.TransactionParseHandler);
            this.handlers.Add("adHocSubProcess", ProcessParseHandler.AdHocSubProcessParseHandler);

            var artifactHandler = new ArtifactParseHandler();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        protected virtual void Init(SubProcess subProcess, IParseContext context, XElement element)
        {
            subProcess.TriggeredByEvent = element.GetBoolean("triggeredByEvent");

            base.Init(subProcess, context, element);
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var subProcess = context.BpmnFactory.CreateSubProcess();
            parent.FlowElements.Add(subProcess);

            this.Init(subProcess, context, element);

            return subProcess;
        }
    }

    class AdHocSubProcessParseHandler : SubProcessParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var adHocSubProcess = context.BpmnFactory.CreateAdHocSubProcess();
            parent.FlowElements.Add(adHocSubProcess);

            base.Init(adHocSubProcess, context, element);

            return adHocSubProcess;
        }
    }

    class TransactionParseHandler : SubProcessParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var transaction = context.BpmnFactory.CreateTransaction();
            parent.FlowElements.Add(transaction);

            base.Init(transaction, context, element);

            return transaction;
        }
    }
}
