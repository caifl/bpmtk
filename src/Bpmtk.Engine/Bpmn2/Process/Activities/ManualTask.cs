using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class ManualTask : Task
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
