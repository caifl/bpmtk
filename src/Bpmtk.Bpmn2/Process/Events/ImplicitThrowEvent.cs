using System;

namespace Bpmtk.Bpmn2
{
    public class ImplicitThrowEvent : ThrowEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
