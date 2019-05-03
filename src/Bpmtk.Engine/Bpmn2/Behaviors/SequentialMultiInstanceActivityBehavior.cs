using System;
using System.Collections;
using System.Collections.Generic;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class SequentialMultiInstanceActivityBehavior : MultiInstanceActivityBehavior
    {
        public SequentialMultiInstanceActivityBehavior(ActivityBehavior innerActivityBehavior,
            MultiInstanceLoopCharacteristics loopCharacteristics) 
            : base(innerActivityBehavior, loopCharacteristics)
        {
        }

        protected virtual void ExecuteOriginalBehavior(ExecutionContext executionContext, int loopCounter)
        {
            var map = new Dictionary<string, object>();
            map.Add("loopCounter", loopCounter);

            var inputDataItem = this.loopCharacteristics.InputDataItem;
            var loopDataInputRef = this.loopCharacteristics.LoopDataInputRef;
            var outputDataItem = this.loopCharacteristics.OutputDataItem;

            if (this.loopCharacteristics.InputDataItem != null && loopDataInputRef != null)
            {
                var itemVarName = inputDataItem.Id;
                var collectionVarName = loopDataInputRef.Id;

                var collection = executionContext.GetVariable(loopDataInputRef.Id);
                if (collection != null && collection is IList)
                {
                    var list = collection as IList;
                    if (list.Count > loopCounter)
                    {
                        var itemValue = list[loopCounter];
                        map[itemVarName] = itemValue;
                    }
                }

                if (outputDataItem != null)
                {
                    //var dataObject = this.OutputDataItem.
                    //map[this.OutputDataItem.Id] = ;
                }
            }

            this.innerActivityBehavior.ExecuteAsync(executionContext).GetAwaiter().GetResult();
        }

        protected override System.Threading.Tasks.Task<int> CreateInstancesAsync(ExecutionContext executionContext)
        {
            var numberOfInstances = this.ResolveNumberOfInstances(executionContext);
            if (numberOfInstances == 0)
                return System.Threading.Tasks.Task.FromResult(numberOfInstances);

            else if (numberOfInstances < 0)
            {
                throw new RuntimeException("Invalid number of instances: must be non-negative integer value"
              + ", but was " + numberOfInstances);
            }

            var context = executionContext.Context;
            var token = executionContext.Token;

            token.IsMIRoot = true;
            token.Inactivate();

            //var store = executionContext.Context.GetService<IInstanceStore>();
            //store.Add(new HistoricToken(executionContext, "activate"));

            var node = executionContext.Node as Activity;
            int numberOfActiveInstances = this.loopCharacteristics.IsSequential ? 1 : numberOfInstances;
            int numberOfCompletedInstances = 0;

            executionContext.SetVariable("numberOfInstances", numberOfInstances, true);
            executionContext.SetVariable("numberOfCompletedInstances", numberOfCompletedInstances, true);
            executionContext.SetVariable("numberOfActiveInstances", numberOfActiveInstances, true);

            if (this.loopCharacteristics.IsSequential)
            {
                var childToken = token.CreateToken();

                var childExecutionContext = ExecutionContext.Create(context, childToken);
                //childExecutionContext.Node = node;
                childExecutionContext.SetVariable("loopCounter", 0, true);

                this.ExecuteOriginalBehavior(childExecutionContext, 0);
            }

            //var tokens = new List<Token>();
            //for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            //{
            //    var instanceToken = token.CreateToken(context);
            //    instanceToken.Node = node;
            //    tokens.Add(instanceToken);
            //}

            //var innerExecutions = new List<ExecutionContext>();

            //IList inputSet = null;
            //var loopDataInputRef = this.LoopDataInputRef;
            //var inputDataItem = this.InputDataItem;
            //if (inputDataItem != null)
            //    inputSet = executionContext.GetVariable<IList>(loopDataInputRef.Id);

            ////创建活动实例
            //for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            //{
            //    var innerToken = tokens[loopCounter];
            //    innerToken.Activate();

            //    var innerExecution = ExecutionContext.Create(context, innerToken);

            //    innerExecution.SetVariable("loopCounter", loopCounter);

            //    if (inputSet != null && loopCounter < inputSet.Count)
            //    {
            //        var inputValue = inputSet[loopCounter];
            //        innerExecution.SetVariable(inputDataItem.Id, inputValue);
            //    }

            //    innerExecutions.Add(innerExecution);
            //}

            ////执行活动实例
            //for (int loopCounter = 0; loopCounter < numberOfInstances; loopCounter++)
            //{
            //    var innerExecution = innerExecutions[loopCounter];

            //    //Execute inner-activity.
            //    this.Activity.Execute(innerExecution);
            //}
            return System.Threading.Tasks.Task.FromResult(numberOfInstances);
        }
    }
}
