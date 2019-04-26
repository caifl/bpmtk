using System;

namespace Bpmtk.Bpmn2
{
    public class Task : Activity
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
