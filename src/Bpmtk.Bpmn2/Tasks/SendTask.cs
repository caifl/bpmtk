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

        public virtual string MessageRef
        {
            get;
            set;
        }

        public virtual string OperationRef
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
