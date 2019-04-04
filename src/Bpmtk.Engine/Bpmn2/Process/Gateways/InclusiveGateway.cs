using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class InclusiveGateway : Gateway
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
