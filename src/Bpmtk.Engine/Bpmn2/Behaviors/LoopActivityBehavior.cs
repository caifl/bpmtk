using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public abstract class LoopActivityBehavior : ActivityBehavior, ILoopActivityBehavior
    {
        protected readonly ActivityBehavior innerActivityBehavior;

        public LoopActivityBehavior(ActivityBehavior innerActivityBehavior)
        {
            this.innerActivityBehavior = innerActivityBehavior;
            this.innerActivityBehavior.LoopActivityBehavior = this;
        }

        public override Task<bool> EvaluatePreConditionsAsync(ExecutionContext executionContext)
        {
            //Mark mi-root.
            var token = executionContext.Token;
            token.IsMIRoot = true;

            return base.EvaluatePreConditionsAsync(executionContext);
        }

        protected abstract Task<int> CreateInstancesAsync(ExecutionContext executionContext);

        public override async Task ExecuteAsync(ExecutionContext executionContext)
        {
            var loopCounter = executionContext.GetVariableLocal("loopCounter");
            if (loopCounter == null)
            {
                int numberOfInstances = 0;

                try
                {
                    numberOfInstances = await this.CreateInstancesAsync(executionContext);
                }
                catch (BpmnError error)
                {
                    throw error;
                    //ErrorPropagation.propagateError(error, execution);
                }

                if (numberOfInstances == 0) 
                {
                    await base.LeaveAsync(executionContext);
                }
            }
            else
            {
                await this.innerActivityBehavior.ExecuteAsync(executionContext);
            }
        }
    }
}
