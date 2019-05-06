using System;
using System.Collections.Generic;
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

        public override bool EvaluatePreConditions(ExecutionContext executionContext)
        {
            //Mark mi-root.
            var token = executionContext.Token;
            token.IsMIRoot = true;

            return base.EvaluatePreConditions(executionContext);
        }

        protected abstract int CreateInstances(ExecutionContext executionContext);

        public override void Execute(ExecutionContext executionContext)
        {
            var loopCounter = executionContext.GetVariableLocal("loopCounter");
            if (loopCounter == null)
            {
                int numberOfInstances = 0;

                try
                {
                    numberOfInstances = this.CreateInstances(executionContext);
                }
                catch (BpmnError error)
                {
                    throw error;
                    //ErrorPropagation.propagateError(error, execution);
                }

                if (numberOfInstances == 0) 
                {
                    base.Leave(executionContext);
                }
            }
            else
            {
                this.innerActivityBehavior.Execute(executionContext);
            }
        }
    }
}
