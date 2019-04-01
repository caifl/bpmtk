using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class UserTaskBehavior : TaskBehavior
    {
        public override void Signal(ExecutionContext executionContext, string signalName, object signalData)
        {
            //check if all task-instances completed.

            //then leave activity.
            this.Leave(executionContext);
        }

        //protected override void Leave(ExecutionContext executionContext)
        //{


        //    base.Leave(executionContext);
        //}
    }
}
