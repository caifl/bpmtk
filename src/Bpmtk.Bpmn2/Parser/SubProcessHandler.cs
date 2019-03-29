using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class SubProcessHandler<TFlowElementContainer, TSubProcess> : ActivityHandler<TFlowElementContainer, TSubProcess>
        where TFlowElementContainer : IFlowElementsContainer
        where TSubProcess : SubProcess
    {
        readonly static SubProcessHandler<IFlowElementsContainer> _SubProcessHandler = new SubProcessHandler<IFlowElementsContainer>();
        readonly static AdHocSubProcessHandler<IFlowElementsContainer> _AdHocSubProcessHandler = new AdHocSubProcessHandler<IFlowElementsContainer>();
        readonly static TransactionHandler<IFlowElementsContainer> _TransactionHandler = new TransactionHandler<IFlowElementsContainer>();

        public SubProcessHandler()
        {
            this.handlers.Add("startEvent", new StartEventHandler<TSubProcess>());
            this.handlers.Add("userTask", new UserTaskHandler<TSubProcess>());
            this.handlers.Add("sequenceFlow", new SequenceFlowHandler<TSubProcess>());
            this.handlers.Add("endEvent", new EndEventHandler<TSubProcess>());
            this.handlers.Add("parallelGateway", new ParallelGatewayHandler<TSubProcess>());
            this.handlers.Add("exclusiveGateway", new ExclusiveGatewayHandler<TSubProcess>());
            this.handlers.Add("inclusiveGateway", new InclusiveGatewayHandler<TSubProcess>());
            this.handlers.Add("scriptTask", new ScriptTaskHandler<TSubProcess>());
            this.handlers.Add("serviceTask", new ServiceTaskHandler<TSubProcess>());
            this.handlers.Add("receiveTask", new ReceiveTaskHandler<TSubProcess>());
            this.handlers.Add("sendTask", new SendTaskHandler<TSubProcess>());
            this.handlers.Add("task", new TaskHandler<TSubProcess>());
            this.handlers.Add("manualTask", new ManualTaskHandler<TSubProcess>());
            this.handlers.Add("businessRuleTask", new BusinessRuleTaskHandler<TSubProcess>());

            this.handlers.Add("subProcess", new BpmnHandlerCallback<TSubProcess>((p, c, x) =>
            {
                return _SubProcessHandler.Create(p, c, x);
            }));

            this.handlers.Add("transaction", new BpmnHandlerCallback<TSubProcess>((p, c, x) =>
            {
                return _TransactionHandler.Create(p, c, x);
            }));

            this.handlers.Add("adHocSubProcess", new BpmnHandlerCallback<TSubProcess>((p, c, x) =>
            {
                return _AdHocSubProcessHandler.Create(p, c, x);
            }));

            this.handlers.Add("dataObjectReference", new DataObjectReferenceHandler<TSubProcess>());

            var artifactHandler = new ArtifactHandler<TSubProcess>();
            this.handlers.Add("textAnnotation", artifactHandler);
            this.handlers.Add("association", artifactHandler);
        }

        public override TSubProcess Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var subProcess = base.Create(parent, context, element);

            subProcess.TriggeredByEvent = element.GetBoolean("triggeredByEvent");

            this.EndScope(context);

            return subProcess;
        }

        protected override void CreateChildren(TSubProcess parent, IParseContext context, XElement element)
        {
            base.CreateChildren(parent, context, element);
        }

        protected virtual void BeginScope(IParseContext context, TSubProcess subProcess)
        {
            var scope = new FlowElementScope(subProcess);
            context.PushScope(scope);
        }

        protected virtual void EndScope(IParseContext context)
        {
            var scope = context.PopScope();
            scope.Complete();
        }
    }

    class SubProcessHandler<TFlowElementContainer> : SubProcessHandler<TFlowElementContainer, SubProcess>
        where TFlowElementContainer : IFlowElementsContainer
    {
        protected override SubProcess New(IParseContext context, XElement element)
        {
            var subProcess = context.BpmnFactory.CreateSubProcess();

            this.BeginScope(context, subProcess);

            return subProcess;
        }
    }

    class AdHocSubProcessHandler<TFlowElementContainer> : SubProcessHandler<TFlowElementContainer, AdHocSubProcess>
        where TFlowElementContainer : IFlowElementsContainer
    {
        protected override AdHocSubProcess New(IParseContext context, XElement element)
        {
            var subProcess = context.BpmnFactory.CreateAdHocSubProcess();

            this.BeginScope(context, subProcess);

            return subProcess;
        }
    }

    class TransactionHandler<TFlowElementContainer> : SubProcessHandler<TFlowElementContainer, Transaction>
            where TFlowElementContainer : IFlowElementsContainer
    {
        protected override Transaction New(IParseContext context, XElement element)
        {
            var subProcess = context.BpmnFactory.CreateTransaction();

            this.BeginScope(context, subProcess);

            return subProcess;
        }
    }
}
