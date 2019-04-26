using System;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class UserTaskActivityBehavior : TaskActivityBehavior, ISignallableActivityBehavior
    {
        public override System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            return base.ExecuteAsync(executionContext);
        }

        public async System.Threading.Tasks.Task SignalAsync(ExecutionContext executionContext, 
            string signalEvent, 
            IDictionary<string, object> signalData)
        {
            var count = await executionContext.GetActiveTaskCountAsync();
            if (count > 0)
                throw new RuntimeException($"There are '{count}' tasks need to be completed.");

            await base.LeaveAsync(executionContext);
        }
    }
}
