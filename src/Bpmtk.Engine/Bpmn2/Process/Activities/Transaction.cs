using System;


namespace Bpmtk.Engine.Bpmn2
{
    public class Transaction : SubProcess
    {
        public Transaction()
        {
            this.Method = "##Compensate";
        }

        public virtual string Method
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
