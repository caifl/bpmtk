using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class ParallelGateway : Gateway
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
