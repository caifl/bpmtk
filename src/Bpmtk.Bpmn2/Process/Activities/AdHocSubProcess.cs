using System;

namespace Bpmtk.Bpmn2
{
    public class AdHocSubProcess : SubProcess
    {
        public virtual Expression CompletionCondition
        {
            get;
            set;
        }

        public virtual bool CancelRemainingInstances
        {
            get;
            set;
        }

        public virtual AdHocOrdering Ordering
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public enum AdHocOrdering
    {
        Parallel,

        Sequential
    }
}
