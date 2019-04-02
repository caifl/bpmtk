using System;

namespace Bpmtk.Bpmn2
{
    public class EndEvent : ThrowEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
