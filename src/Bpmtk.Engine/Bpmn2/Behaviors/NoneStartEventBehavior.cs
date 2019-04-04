using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class NoneStartEventBehavior : FlowNodeBehavior
    {
        public override void Execute(ExecutionContext executionContext)
        {
            var act = executionContext.Token.Activity;
            //act.PullInputs();

            base.Execute(executionContext);
        }
    }
}
