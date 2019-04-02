using System;

namespace Bpmtk.Bpmn2
{
    public class IntermediateCatchEvent : CatchEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
