using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class CallActivity : Activity
    {
        public virtual string CalledElement
        {
            get;
            set;
        }

        public virtual CallableElement CalledElementRef
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
