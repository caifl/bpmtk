using System;

namespace Bpmtk.Bpmn2
{
    public abstract class LoopCharacteristics : BaseElement
    {
        public virtual Activity Activity
        {
            get;
            set;
        }

        //protected abstract int ResolveNumberOfInstances(ExecutionContext executionContext);
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
    }
}
