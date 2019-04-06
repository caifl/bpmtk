using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class SubProcessParseHandler : ActivityParseHandler
    {
        readonly static SubProcessParseHandler _SubProcessHandler = new SubProcessParseHandler();
        readonly static AdHocSubProcessParseHandler _AdHocSubProcessHandler = new AdHocSubProcessParseHandler();
        readonly static TransactionParseHandler _TransactionHandler = new TransactionParseHandler();

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

            //var handler = processParserHandler.SubProcessParseHandler;

            //this.handlers.Add("subProcess", handler);
            //this.handlers.Add("transaction", ProcessParseHandler.TransactionParseHandler);
            //this.handlers.Add("adHocSubProcess", ProcessParseHandler.AdHocSubProcessParseHandler);
            this.handlers.Add("subProcess", new BpmnHandlerCallback((p, c, x) =>
            {
                return _SubProcessHandler.Create(p, c, x);
            }));

            this.handlers.Add("transaction", new BpmnHandlerCallback((p, c, x) =>
            {
                return _TransactionHandler.Create(p, c, x);
            }));

            this.handlers.Add("adHocSubProcess", new BpmnHandlerCallback((p, c, x) =>
            {
                return _AdHocSubProcessHandler.Create(p, c, x);
            }));

            var artifactHandler = new ArtifactParseHandler();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        class BpmnHandlerCallback : ParseHandler<IFlowElementsContainer>
        {
            private readonly Func<IFlowElementsContainer, IParseContext, XElement, object> action;

            public BpmnHandlerCallback(Func<IFlowElementsContainer, IParseContext, XElement, object> action)
            {
                this.action = action;
            }

            public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
            {
                return action(parent, context, element);
            }
        }

        protected override void Init(BaseElement basElement, IParseContext context, XElement element)
        {
            var subProcess = basElement as SubProcess;
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
