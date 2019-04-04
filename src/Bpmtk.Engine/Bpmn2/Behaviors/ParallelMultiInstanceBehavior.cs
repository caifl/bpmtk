using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class ParallelMultiInstanceBehavior : MultiInstanceActivityBehavior
    {
        protected readonly MultiInstanceLoopCharacteristics loopCharacteristics;

        public ParallelMultiInstanceBehavior(Activity activity, BpmnActivityBehavior innerActivityBehavior) 
            : base(activity, innerActivityBehavior)
        {
            this.loopCharacteristics = activity.LoopCharacteristics as MultiInstanceLoopCharacteristics;
        }

        protected override bool IsCompleted(ExecutionContext executionContext)
        {
            var loop = this.activity.LoopCharacteristics as MultiInstanceLoopCharacteristics;

            if (loop.CompletionCondition == null
                || string.IsNullOrEmpty(loop.CompletionCondition.Text))
                return false;

            var condition = loop.CompletionCondition.Text;

            var evaluator = executionContext.CreateExpressionEvaluator();
            var result = evaluator.Evaluate<bool>(condition);
            return result;
        }

        protected virtual int ResolveNumberOfInstances(ExecutionContext executionContext)
        {
            var loopCharacteristics = this.activity.LoopCharacteristics as MultiInstanceLoopCharacteristics;
            var expressionText = loopCharacteristics.LoopCardinality?.Text;
            var loopDataInputRef = loopCharacteristics.LoopDataInputRef;
            int count = -1;

            if (!string.IsNullOrEmpty(expressionText))
            {
                var evaluator = executionContext.CreateExpressionEvaluator();
                var value = evaluator.Evaluate<int?>(expressionText);
                if (value != null)
                    count = Convert.ToInt32(value);
            }
            else if(loopDataInputRef != null)
            {
                var loopCollection = executionContext.GetVariable(loopDataInputRef.Id);
                if (loopCollection != null && loopCollection is ICollection)
                    count = ((ICollection)loopCollection).Count;
            }

            return count;
        }

        protected override int CreateInstances(ExecutionContext executionContext)
        {
            var numberOfInstances = this.ResolveNumberOfInstances(executionContext);
            if (numberOfInstances < 0)
            {
                throw new RuntimeException("Invalid number of instances: must be non-negative integer value"
              + ", but was " + numberOfInstances);
            }

            executionContext.SetVariable("numberOfInstances", numberOfInstances);
            executionContext.SetVariable("numberOfCompletedInstances", 0);
            executionContext.SetVariable("numberOfActiveInstances", numberOfInstances);

            var token = executionContext.Token;
            var node = token.Activity;

            var tokens = new List<Token>();
            for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            {
                var instanceToken = token.CreateToken();
                instanceToken.Activity = node;
                tokens.Add(instanceToken);
            }

            var innerExecutions = new List<ExecutionContext>();

            IList inputSet = null;
            var loopDataInputRef = this.loopCharacteristics.LoopDataInputRef;
            var inputDataItem = this.loopCharacteristics.InputDataItem;
            if (inputDataItem != null)
                inputSet = executionContext.GetVariable<IList>(loopDataInputRef.Id);

            //创建活动实例
            for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            {
                var innerToken = tokens[loopCounter];
                innerToken.Activate();

                var innerExecution = new ExecutionContext(innerToken);

                //await this.CreateAsync(innerExecution);

                innerExecution.SetVariable("loopCounter", loopCounter);

                if (inputSet != null && loopCounter < inputSet.Count)
                {
                    var inputValue = inputSet[loopCounter];
                    innerExecution.SetVariable(inputDataItem.Id, inputValue);
                }

                //await this.ActivatingAsync(innerExecution);

                innerExecutions.Add(innerExecution);
            }

            //执行活动实例
            for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            {
                var innerExecution = innerExecutions[loopCounter];

                this.innerActivityBehavior.Execute(innerExecution);
            }

            return numberOfInstances;
        }

        public override void Leave(ExecutionContext executionContext)
        {
            //设置活动实例为结束状态
            var token = executionContext.Token;            
            token.Inactivate();

            var parentToken = token.Parent;
            if(parentToken != null)
            {
                var parentExecution = new ExecutionContext(parentToken);

                var numberOfInstances = parentExecution.GetVariable<int>("numberOfInstances");
                var numberOfCompletedInstances = parentExecution.GetVariable<int>("numberOfCompletedInstances") + 1;
                var numberOfActiveInstances = parentExecution.GetVariable<int>("numberOfActiveInstances") - 1;

                parentExecution.SetVariable("numberOfCompletedInstances", numberOfCompletedInstances);
                parentExecution.SetVariable("numberOfActiveInstances", numberOfActiveInstances);

                //输出变量到集合
                var loopOutputRef = this.loopCharacteristics.LoopDataOutputRef;
                if (loopOutputRef != null)
                {
                    var loopCounter = executionContext.GetVariable<int>("loopCounter");
                    var outputSet = parentExecution.GetVariable<IList>(loopOutputRef.Id);

                    var outputRef = this.loopCharacteristics.OutputDataItem?.Id;
                    if (!string.IsNullOrEmpty(outputRef) && loopCounter < outputSet.Count)
                    {
                        var result = executionContext.GetVariable(outputRef);
                        outputSet[loopCounter] = result;

                        parentExecution.SetVariable(loopOutputRef.Id, outputSet);
                    }
                }

                var node = token.Activity;
                var inactivateTokens = parentToken.Children.Where(x => !x.IsActive).ToList();
                if (inactivateTokens.Count() >= numberOfInstances
                    || this.IsCompleted(parentExecution))
                {
                    //remove all child tokens
                    foreach (var item in inactivateTokens)
                        item.Remove();

                    base.Leave(parentExecution);
                }
            }
            else
                base.Leave(executionContext);
        }
    }
}
