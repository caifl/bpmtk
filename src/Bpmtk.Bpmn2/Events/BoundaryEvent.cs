using System;

namespace Bpmtk.Bpmn2
{
    public class BoundaryEvent : CatchEvent
    {
        public virtual bool CancelActivity
        {
            get;
            set;
        }

        public virtual string AttachedToRef
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
