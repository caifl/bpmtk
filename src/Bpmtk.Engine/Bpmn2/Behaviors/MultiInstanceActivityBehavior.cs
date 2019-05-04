using System;
using System.Collections;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    abstract class MultiInstanceActivityBehavior : LoopActivityBehavior
    {
        protected readonly MultiInstanceLoopCharacteristics loopCharacteristics;

        public MultiInstanceActivityBehavior(ActivityBehavior innerActivityBehavior,
            MultiInstanceLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior)
        {
            this.loopCharacteristics = loopCharacteristics;
        }

        //public override Task<bool> EvaluatePreConditionsAsync(ExecutionContext executionContext)
        //{
        //    //
        //    var instances = this.ResolveNumberOfInstances(executionContext);
        //    if(instances > 0)
        //    {
        //        var token = executionContext.Token;
        //        token.IsMIRoot = true;
        //    }

        //    return base.EvaluatePreConditionsAsync(executionContext);
        //}

        protected virtual bool IsCompleted(ExecutionContext executionContext)
        {
            if (loopCharacteristics.CompletionCondition == null)
                return false;

            var condition = loopCharacteristics.CompletionCondition.Text;
            var result = executionContext.GetEvaluator().Evaluate(condition);
            if (result != null)
                return Convert.ToBoolean(result);

            return false;
        }

        /// <summary>
        /// In order to initialize a valid multi-instance, either the
        /// loopCardinality Expression or the loopDataInput MUST be specified.
        /// </summary>
        protected virtual int ResolveNumberOfInstances(ExecutionContext executionContext)
        {
            if (loopCharacteristics.LoopCardinality == null && loopCharacteristics.LoopDataInputRef == null)
                throw new RuntimeException("The multi-instance either the loopCardinality Expression or the loopDataInput MUST be specified.");

            var loopDataInputRef = loopCharacteristics.LoopDataInputRef;
            int count = -1;

            if (loopCharacteristics.LoopCardinality != null)
            {
                var value = executionContext.GetEvaluator().Evaluate(loopCharacteristics.LoopCardinality.Text);
                if (value != null)
                    count = Convert.ToInt32(value);
            }
            else if (loopDataInputRef != null)
            {
                var loopCollection = executionContext.GetVariable(loopDataInputRef.Id);
                if (loopCollection != null && loopCollection is ICollection)
                    count = ((ICollection)loopCollection).Count;
            }

            return count;
        }

        
    }
}
