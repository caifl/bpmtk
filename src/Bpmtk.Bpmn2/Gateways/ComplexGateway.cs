using System;

namespace Bpmtk.Bpmn2
{
    public class ComplexGateway : Gateway
    {
        public virtual Expression ActivationCondition
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
