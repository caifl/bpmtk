using System;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class CallActivity : Activity
    {
        public virtual string CalledElement
        {
            get;
            set;
        }

        public virtual CallableElement CalledElementRef
        {
            get;
            set;
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var context = executionContext.Context;
            var token = executionContext.Token;

            var subProcessInstance = token.CreateSubProcessInstance(context);
            subProcessInstance.Start(context, null);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
