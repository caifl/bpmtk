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

        protected abstract Task<int> CreateInstancesAsync(ExecutionContext executionContext);

        public override async Task ExecuteAsync(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
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

            if (numberOfInstances == 0) //实例数量为零的情况下仍然建立一个活动实例, 只是不执行该节点的任何行为
            {
                await this.innerActivityBehavior.ExecuteAsync(executionContext);
                //this.OnActivating(executionContext);

                //this.OnActivated(executionContext);

                ////ignore activity behavior.
                ////base.Execute(executionContext);

                //base.LeaveDefault(executionContext);
            }
        }
    }
}
