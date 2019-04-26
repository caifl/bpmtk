using System;

namespace Bpmtk.Bpmn2
{
    public class ParallelGateway : Gateway
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
