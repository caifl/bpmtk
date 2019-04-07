using System;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class ReceiveTask : Task
    {
        public virtual string Implementation
        {
            get;
            set;
        }

        public virtual bool Instantiate
        {
            get;
            set;
        }

        public virtual Message MessageRef
        {
            get;
            set;
        }

        public virtual Operation OperationRef
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            //Waiting ...

            //base.Execute(executionContext);
        }

        public override void Signal(ExecutionContext executionContext, string signalName, object signalData)
        {
            base.Leave(executionContext);
            //base.Signal(executionContext, signalName, signalData);
        }
    }
}
