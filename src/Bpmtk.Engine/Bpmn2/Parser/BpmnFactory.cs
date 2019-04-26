using System;
using Bpmtk.Bpmn2;
using Bpmtk.Bpmn2.DI;
using Bpmtk.Bpmn2.DC;
using Bpmtk.Engine.Bpmn2.Behaviors;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class BpmnFactory
    {
        public virtual Definitions CreateDefinitions()
        {
            return new Definitions();
        }

        #region RootElements

        public virtual Process CreateProcess()
        {
            return new Process();
        }

        public virtual Resource CreateResource()
        {
            return new Resource();
        }

        public virtual ItemDefinition CreateItemDefinition()
        {
            return new ItemDefinition();
        }

        public virtual Signal CreateSignal() => new Signal();

        public virtual Error CreateError() => new Error();

        public virtual Interface CreateInterface() => new Interface();

        public virtual DataStore CreateDataStore() => new DataStore();

        public virtual Message CreateMessage() => new Message();

        public virtual Import CreateImport() => new Import();

        #endregion

        public virtual Property CreateProperty() => new Property();

        #region FlowElements

        public virtual StartEvent CreateStartEvent()
        {
            var startEvent = new StartEvent();
            startEvent.Tag = new StartEventActivityBehavior();

            return startEvent;
        }

        public virtual EndEvent CreateEndEvent()
        {
            var endEvent = new EndEvent();
            endEvent.Tag = new EndEventActivityBehavior();

            return endEvent;
        }

        public virtual DataObject CreateDataObject() => new ValuedDataObject();

        public virtual SequenceFlow CreateSequenceFlow() => new SequenceFlow();

        public virtual UserTask CreateUserTask()
        {
            var task = new UserTask();
            task.Tag = new UserTaskActivityBehavior();
            return task;
        }

        public virtual BusinessRuleTask CreateBusinessRuleTask()
        {
            var task = new BusinessRuleTask();
            task.Tag = new BusinessRuleTaskActivityBehavior();

            return task;
        }

        public virtual ScriptTask CreateScriptTask()
        {
            var task = new ScriptTask();
            task.Tag = new ScriptTaskActivityBehavior();

            return task;
        }

        public virtual Task CreateTask()
        {
            var task = new Task();
            task.Tag = new TaskActivityBehavior();

            return task;
        }

        public virtual ServiceTask CreateServiceTask()
        {
            var task = new ServiceTask();
            task.Tag = new ServiceTaskActivityBehavior();

            return task;
        }

        public virtual SendTask CreateSendTask()
        {
            var task = new SendTask();
            task.Tag = new SendTaskActivityBehavior();

            return task;
        }

        public virtual ReceiveTask CreateReceiveTask()
        {
            var task = new ReceiveTask();
            task.Tag = new ReceiveTaskActivityBehavior();

            return task;
        }

        public virtual ManualTask CreateManualTask()
        {
            var task = new ManualTask();
            task.Tag = new ManualTaskActivityBehavior();

            return task;
        }

        public virtual SubProcess CreateSubProcess()
        {
            var subProcess = new SubProcess();
            subProcess.Tag = new SubProcessActivityBehavior();

            return subProcess;
        }

        public virtual Transaction CreateTransaction()
        {
            var trans = new Transaction();
            trans.Tag = new TransactionActivityBehavior();

            return trans;
        }

        public virtual AdHocSubProcess CreateAdHocSubProcess()
        {
            var adHocSubProcess = new AdHocSubProcess();
            adHocSubProcess.Tag = new AdHocSubProcessActivityBehavior();

            return adHocSubProcess;
        }

        public virtual CallActivity CreateCallActivity()
        {
            var callActivity = new CallActivity();
            callActivity.Tag = new CallActivityBehavior();

            return callActivity;
        }

        #endregion

        #region Gateways

        public virtual ExclusiveGateway CreateExclusiveGateway()
        {
            var gateway = new ExclusiveGateway();
            gateway.Tag = new ExclusiveGatewayActivityBehavior();

            return gateway;
        }

        public virtual InclusiveGateway CreateInclusiveGateway()
        {
            var gateway = new InclusiveGateway();
            gateway.Tag = new InclusiveGatewayActivityBehavior();

            return gateway;
        }

        public virtual ParallelGateway CreateParallelGateway()
        {
            var gateway = new ParallelGateway();
            gateway.Tag = new ParallelGatewayActivityBehavior();

            return gateway;
        }

        public virtual ComplexGateway CreateComplexGateway()
        {
            var gateway = new ComplexGateway();
            gateway.Tag = new ComplexGatewayActivityBehavior();

            return gateway;
        }

        public virtual EventBasedGateway CreateEventBasedGateway()
        {
            var gateway = new EventBasedGateway();
            gateway.Tag = new EventBasedGatewayActivityBehavior();

            return gateway;
        }

        #endregion

        #region Data

        public virtual InputOutputBinding CreateInputOutputBinding() => new InputOutputBinding();

        public virtual DataState CreateDataState() => new DataState();

        public virtual InputOutputSpecification CreateIOSpecification() => new InputOutputSpecification();

        public virtual DataInput CreateDataInput() => new DataInput();

        public virtual DataOutput CreateDataOutput() => new DataOutput();

        public virtual InputSet CreateInputSet() => new InputSet();

        public virtual OutputSet CreateOutputSet() => new OutputSet();

        public virtual DataInputAssociation CreateDataInputAssociation() => new DataInputAssociation();

        public virtual DataOutputAssociation CreateDataOutputAssociation() => new DataOutputAssociation();

        public virtual DataObjectReference CreateDataObjectReference() => new DataObjectReference();

        public virtual Assignment CreateAssignment() => new Assignment();

        #endregion

        #region Diagrams

        public virtual BPMNDiagram CreateDiagram() => new BPMNDiagram();

        public virtual BPMNPlane CreatePlane() => new BPMNPlane();

        public virtual BPMNShape CreateShape() => new BPMNShape();

        public virtual BPMNLabel CreateLabel() => new BPMNLabel();

        public virtual BPMNEdge CreateEdge() => new BPMNEdge();

        public virtual Bounds CreateBounds() => new Bounds();

        public virtual Point CreatePoint() => new Point();

        #endregion

        public virtual TextAnnotation CreateTextAnnotation() => new TextAnnotation();

        public virtual Association CreateAssociation() => new Association();

        public virtual MultiInstanceLoopCharacteristics CreateMultiInstanceLoopCharacteristics() => new MultiInstanceLoopCharacteristics();

        public virtual StandardLoopCharacteristics CreateStandardLoopCharacteristics() => new StandardLoopCharacteristics();

        public virtual Operation CreateOperation() => new Operation();

        public virtual Expression CreateExpression() => new Expression();

        public virtual FormalExpression CreateFormalExpression() => new FormalExpression();

        public virtual PotentialOwner CreatePotentialOwner() => new PotentialOwner();

        public virtual HumanPerformer CreateHumanPerformer() => new HumanPerformer();

        public virtual Performer CreatePerformer() => new Performer();

        public virtual ResourceRole CreateResourceRole() => new ResourceRole();

        public virtual ResourceParameter CreateResourceParameter()
        {
            return new ResourceParameter();
        }

        public virtual ResourceParameterBinding CreateResourceParameterBinding() => new ResourceParameterBinding();

        public virtual ResourceAssignmentExpression CreateResourceAssignmentExpression() => new ResourceAssignmentExpression();

        public virtual Documentation CreateDocumentation()
        {
            return new Documentation();
        }

        public virtual ExtensionElements CreateExtensionElements() => new ExtensionElements();
    }
}
