using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class ServiceTask : Task
    { 
        public virtual string Implementation
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
