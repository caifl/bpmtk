using System;
using Bpmtk.Engine.Bpmn2.DI;
using Bpmtk.Engine.Bpmn2.DC;

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

        public virtual StartEvent CreateStartEvent() => new StartEvent();

        public virtual EndEvent CreateEndEvent() => new EndEvent();

        public virtual DataObject CreateDataObject() => new ValuedDataObject();

        public virtual SequenceFlow CreateSequenceFlow() => new SequenceFlow();

        public virtual UserTask CreateUserTask() => new UserTask();

        public virtual BusinessRuleTask CreateBusinessRuleTask() => new BusinessRuleTask();

        public virtual ScriptTask CreateScriptTask() => new ScriptTask();

        public virtual Task CreateTask() => new Task();

        public virtual ServiceTask CreateServiceTask() => new ServiceTask();

        public virtual SendTask CreateSendTask() => new SendTask();

        public virtual ReceiveTask CreateReceiveTask() => new ReceiveTask();

        public virtual ManualTask CreateManualTask() => new ManualTask();

        public virtual SubProcess CreateSubProcess() => new SubProcess();

        public virtual Transaction CreateTransaction() => new Transaction();

        public virtual AdHocSubProcess CreateAdHocSubProcess() => new AdHocSubProcess();

        #endregion

        #region Gateways

        public virtual ExclusiveGateway CreateExclusiveGateway() => new ExclusiveGateway();

        public virtual InclusiveGateway CreateInclusiveGateway() => new InclusiveGateway();

        public virtual ParallelGateway CreateParallelGateway() => new ParallelGateway();

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
