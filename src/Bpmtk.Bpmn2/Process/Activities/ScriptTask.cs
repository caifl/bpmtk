using System;

namespace Bpmtk.Bpmn2
{
    public class ScriptTask : Task
    {
        public virtual string Script
        {
            get;
            set;
        }

        public virtual string ScriptFormat
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
