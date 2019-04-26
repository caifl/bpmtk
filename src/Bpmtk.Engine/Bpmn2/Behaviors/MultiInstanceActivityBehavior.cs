using System;
using System.Collections;
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

        protected abstract int CreateInstances(ExecutionContext executionContext);

        protected virtual bool IsCompleted(ExecutionContext executionContext)
        {
            if (loopCharacteristics.CompletionCondition == null)
                return false;

            var condition = loopCharacteristics.CompletionCondition.Text;
            var result = executionContext.EvaluteExpression(condition);
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
                var value = executionContext.EvaluteExpression(loopCharacteristics.LoopCardinality.Text);
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

        public override async System.Threading.Tasks.Task LeaveAsync(ExecutionContext executionContext)
        {
            //设置活动实例为结束状态
            var context = executionContext.Context;
            var token = executionContext.Token;

            var parentToken = token.Parent;
            if (parentToken == null || !parentToken.IsMIRoot)
                throw new RuntimeException("Invalid multiInstance execution.");

            if (!this.loopCharacteristics.IsSequential)
                token.Inactivate();

            //fire inner activity leave event.
            //this.Activity.OnInnerActivityEnded(executionContext);

            var parentExecution = ExecutionContext.Create(context, parentToken);

            var numberOfInstances = parentExecution.GetVariableLocal<int>("numberOfInstances");
            var numberOfCompletedInstances = parentExecution.GetVariableLocal<int>("numberOfCompletedInstances") + 1;
            var numberOfActiveInstances = parentExecution.GetVariableLocal<int>("numberOfActiveInstances");

            if (!this.loopCharacteristics.IsSequential)
            {
                numberOfActiveInstances += 1;
                parentExecution.SetVariable("numberOfActiveInstances", numberOfActiveInstances);
            }

            parentExecution.SetVariableLocal("numberOfCompletedInstances", numberOfCompletedInstances);

            var loopCounter = executionContext.GetVariableLocal<int>("loopCounter");

            var loopDataOutputRef = this.loopCharacteristics.LoopDataOutputRef;
            var outputDataItem = this.loopCharacteristics.OutputDataItem;

            //Update outputDataItem
            var loopOutputRef = loopDataOutputRef;
            if (loopOutputRef != null && outputDataItem != null)
            {
                var collection = executionContext.GetVariable(loopOutputRef.Id);
                if (collection != null && collection is IList)
                {
                    var itemVarName = outputDataItem.Id;
                    var list = collection as IList;
                    if (list.Count > loopCounter)
                    {
                        var itemValue = executionContext.GetVariable(itemVarName);
                        list[loopCounter] = itemValue;
                        executionContext.SetVariable(loopOutputRef.Id, itemValue);
                    }
                }
            }

            loopCounter += 1;
            if (loopCounter >= numberOfInstances || this.IsCompleted(parentExecution))
            {
                //Remove token.
                token.Remove();

                //exit multi-instance loop activity.
                parentToken.IsMIRoot = false;
                parentToken.Activate();

                //leave without check loop.
                //this.Activity.LeaveDefault(parentExecution);
                await base.LeaveAsync(executionContext);
            }
            else
            {
                executionContext.SetVariableLocal("loopCounter", loopCounter);
                //this.ExecuteOriginalBehavior(executionContext, loopCounter);
            }

            //var node = token.Node;
            //var inactivateTokens = parentToken.Children.Where(x => !x.IsActive).ToList();
            //if (inactivateTokens.Count() >= numberOfInstances
            //    || this.IsCompleted(parentExecution))
            //{
            //    //remove all child tokens
            //    foreach (var item in inactivateTokens)
            //        item.Remove(context);

            //    base.Leave(parentExecution);
            //}
        }
    }
}
