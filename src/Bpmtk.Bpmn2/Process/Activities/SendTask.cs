using System;

namespace Bpmtk.Bpmn2
{
    public class SendTask : Task
    {
        public virtual string Implementation
        {
            get;
            set;
        }

        public virtual Message MessageRef
        {
            get;
            set;
        }

        public virtual Operation OperationRef
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
