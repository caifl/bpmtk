using System;

namespace Bpmtk.Bpmn2
{
    public class ExclusiveGateway : Gateway
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
