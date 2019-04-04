using Bpmtk.Engine.Runtime;
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
            var numberOfInstances = this.ResolveNumberOfInstances(executionContext);
            if (numberOfInstances == 0)
            {
                //try
                //{
                    numberOfInstances = this.CreateInstances(executionContext);
                //}
                //catch (BpmnError error)
                //{
                //    ErrorPropagation.propagateError(error, execution);
                //}

                if (numberOfInstances == 0)
                {
                    this.Activity.Leave(executionContext);
                }
            }
            else
            {
                //recordActivityStart((ExecutionEntity)executionContext);
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
