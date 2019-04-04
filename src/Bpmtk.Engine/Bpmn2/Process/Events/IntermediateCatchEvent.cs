using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class IntermediateCatchEvent : CatchEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
