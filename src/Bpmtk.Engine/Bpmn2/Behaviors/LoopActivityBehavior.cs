using System;
using System.Collections.Generic;
using System.Text;
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

        //public override void Execute(ExecutionContext executionContext)
        //{
        //    //var token = executionContext.Token;
        //    //int numberOfInstances = 0;

        //    //try
        //    //{
        //    //    numberOfInstances = this.LoopCharacteristics.CreateInstances(executionContext);
        //    //}
        //    //catch (BpmnError error)
        //    //{
        //    //    throw error;
        //    //    //ErrorPropagation.propagateError(error, execution);
        //    //}

        //    //if (numberOfInstances == 0) //实例数量为零的情况下仍然建立一个活动实例, 只是不执行该节点的任何行为
        //    //{
        //    //    this.OnActivating(executionContext);

        //    //    this.OnActivated(executionContext);

        //    //    //ignore activity behavior.
        //    //    //base.Execute(executionContext);

        //    //    base.LeaveDefault(executionContext);
        //    //}
        //}
    }
}
