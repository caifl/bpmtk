using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class BusinessRuleTask : Task
    {
        public BusinessRuleTask()
        {
            this.Implementation = "##WebService";
        }

        public virtual string Implementation
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
