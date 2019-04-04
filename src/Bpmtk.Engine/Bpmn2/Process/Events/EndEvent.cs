using System;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class EndEvent : ThrowEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            token.End();
        }
    }
}
