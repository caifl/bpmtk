using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class CallActivityBehavior : ActivityBehavior
    {
        //public override void Execute(ExecutionContext executionContext)
        //{
        //    var callActivity = executionContext.Node as CallActivity;

        //    var process = callActivity.CalledElementRef;
        //    var subProc = executionContext.CreateSubProcessContext();
        //    subProc.Start(null);
        //}

        public virtual void Completing(ExecutionContext executionContext,
            ExecutionContext subProcessExecutionContext)
        {

        }
    }
}
