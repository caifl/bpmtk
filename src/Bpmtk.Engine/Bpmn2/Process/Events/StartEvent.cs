using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    /// <summary>
    /// As the name implies, the Start Event indicates where a particular Process will start. In terms of Sequence Flows, the
    /// Start Event starts the flow of the Process, and thus, will not have any incoming Sequence Flows—no Sequence
    /// Flow can connect to a Start Event.
    /// </summary>
    public class StartEvent : CatchEvent
    {
        /// <summary>
        /// This attribute only applies to Start Events of Event Sub-Processes; it is ignored for
        /// other Start Events.This attribute denotes whether the Sub-Process encompassing
        /// the Event Sub-Process should be cancelled or not, If the encompassing Sub-
        /// Process is not cancelled, multiple instances of the Event Sub-Process can run
        /// concurrently.This attribute cannot be applied to Error Events (where it’s always
        /// true), or Compensation Events(where it doesn’t apply).
        /// </summary>
        public virtual bool IsInterrupting
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Enter(ExecutionContext executionContext)
        {
            this.Execute(executionContext);
            //throw new BpmnError("The startEvent not supported enter.");
        }

        public override void Execute(ExecutionContext executionContext)
        {
            //create act-inst
            //executionContext.ActivityInstance = base.CreateActivityInstance(executionContext);

            //activate act-inst and fire event.
            this.OnActivate(executionContext);

            //Push DataOutputs.
            //this.DataOutputAssociations
            base.Leave(executionContext);
        }
    }
}
