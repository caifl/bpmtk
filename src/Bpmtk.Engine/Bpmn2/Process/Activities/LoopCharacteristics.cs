using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;
using System;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class LoopCharacteristics : BaseElement
    {
        public virtual Activity Activity
        {
            get;
            set;
        }

        public virtual void Execute(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            token.Node = this.Activity;

            

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

                if (numberOfInstances == 0) //实例数量为零的情况下仍然建立一个活动实例, 只是不执行该节点的任何行为
                {
                    var act = ActivityInstance.Create(executionContext);

                    executionContext.ActivityInstance = act;

                    //fire nodeEnter event
                    var store = executionContext.Context.GetService<IInstanceStore>();
                    store.Add(new HistoricToken(executionContext, "enter"));

                    act.Activate();
                    store.Add(new HistoricToken(executionContext, "activate"));

                    // remove the transition references from the runtime context
                    executionContext.Transition = null;
                    executionContext.TransitionSource = null;

                    this.Activity.LeaveDefault(executionContext);
                }
            }
            else
            {
                //fire ActivityInstance activated event.
                var act = ActivityInstance.Create(executionContext);

                executionContext.ActivityInstance = act;
                act.Activate();

                var store = executionContext.Context.GetService<IInstanceStore>();
                store.Add(new HistoricToken(executionContext, "activate"));

                this.Activity.Execute(executionContext);
            }
        }

        public virtual void Leave(ExecutionContext executionContext)
        {
            this.Activity.Leave(executionContext);
        }

        protected abstract int ResolveNumberOfInstances(ExecutionContext executionContext);
        //{
        //    var expressionText = this.LoopCardinality?.Text;
        //    var loopDataInputRef = this.LoopDataInputRef;
        //    int count = -1;

        //    if (!string.IsNullOrEmpty(expressionText))
        //    {
        //        var evaluator = executionContext.CreateExpressionEvaluator();
        //        var value = evaluator.Evaluate<int?>(expressionText);
        //        if (value != null)
        //            count = Convert.ToInt32(value);
        //    }
        //    else if (loopDataInputRef != null)
        //    {
        //        var loopCollection = executionContext.GetVariable(loopDataInputRef.Id);
        //        if (loopCollection != null && loopCollection is ICollection)
        //            count = ((ICollection)loopCollection).Count;
        //    }

        //    return count;
        //}

        protected abstract int CreateInstances(ExecutionContext executionContext);

        protected abstract bool IsCompleted(ExecutionContext executionContext);
    }
}
