using System;

namespace Bpmtk.Bpmn2
{
    public class ManualTask : Task
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
