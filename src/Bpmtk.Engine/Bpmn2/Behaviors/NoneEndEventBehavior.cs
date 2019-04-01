using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class NoneEndEventBehavior : FlowNodeBehavior
    {
        public NoneEndEventBehavior()
        {
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            token.End();
        }
    }
}
