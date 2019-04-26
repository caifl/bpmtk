using System;

namespace Bpmtk.Bpmn2
{
    public interface IFlowNodeVisitor : IActivityVisitor
    {
        #region Events

        void Visit(StartEvent startEvent);

        void Visit(EndEvent endEvent);

        void Visit(BoundaryEvent boundryEvent);

        void Visit(ImplicitThrowEvent value);

        void Visit(IntermediateCatchEvent value);

        void Visit(IntermediateThrowEvent value); 

        #endregion

        #region Gateways

        void Visit(ExclusiveGateway value);

        void Visit(InclusiveGateway value);

        void Visit(ParallelGateway value);

        void Visit(ComplexGateway value);

        void Visit(EventBasedGateway value);

        #endregion
    }

    public interface IActivityVisitor
    {
        void Visit(AdHocSubProcess adHocSubProcess);

        void Visit(SubProcess subProcess);

        void Visit(Transaction transaction);

        void Visit(CallActivity value);

        #region Tasks

        void Visit(UserTask userTask);

        void Visit(ManualTask value);

        void Visit(Task value);

        void Visit(ReceiveTask value);

        void Visit(SendTask value);

        void Visit(ScriptTask scriptTask);

        void Visit(ServiceTask value);

        void Visit(BusinessRuleTask value);

        #endregion
    }
}
