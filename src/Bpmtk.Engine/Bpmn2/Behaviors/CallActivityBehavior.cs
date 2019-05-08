using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class CallActivityBehavior : ActivityBehavior
    {
        public override void Execute(ExecutionContext executionContext)
        {
            var callActivity = executionContext.Node as CallActivity;

            var process = callActivity.CalledElementRef as Process;
            if (process == null)
                throw new NotSupportedException("The called target element not supported.");

            //var argumentsIn = new List<string>();
            //foreach(var ioBinding in process.IOBindings)
            //{
            //    var inputSet = ioBinding.InputDataRef;
            //}
           
            var calledElementContext = executionContext.CreateCalledElementContext(process);

            //pass input arguments.
            //object value = null;
            //foreach(var argumentName in argumentsIn)
            //{
            //    if (executionContext.TryGetVariable(argumentName, out value))
            //    {
            //        calledElementContext.SetVariableLocal(argumentName, value);
            //    }
            //}

            calledElementContext.Start();
        }

        public virtual void CalledElementEnded(ExecutionContext executionContext,
            ExecutionContext calledElementContext)
        {
            //Get resultSet from subProcessInstance.

            //Clear
            executionContext.SubProcessInstance = null;

            this.Leave(executionContext);
        }
    }
}
