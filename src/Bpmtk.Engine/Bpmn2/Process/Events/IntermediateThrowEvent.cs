
using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class IntermediateThrowEvent : ThrowEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
