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

        protected virtual async System.Threading.Tasks.Task ExecuteOriginalBehaviorAsync(ExecutionContext executionContext, int loopCounter)
        {
            var engine = executionContext.Engine;

            //fire activityReadEvent.
            await engine.ProcessEventListener.ActivityReadyAsync(executionContext);

            var map = new Dictionary<string, object>();
            //map.Add("loopCounter", loopCounter);

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

            //var em = map.GetEnumerator();
            //while(em.MoveNext())
            //{
            //    executionContext.SetVariableLocal()
            //}

            //fire activityStartEvent.
            await engine.ProcessEventListener.ActivityStartAsync(executionContext);

            await this.innerActivityBehavior.ExecuteAsync(executionContext);
        }

        protected override async System.Threading.Tasks.Task<int> CreateInstancesAsync(ExecutionContext executionContext)
        {
            var numberOfInstances = this.ResolveNumberOfInstances(executionContext);
            if (numberOfInstances == 0)
                return numberOfInstances;

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
            int numberOfActiveInstances = 1;
            int numberOfCompletedInstances = 0;

            executionContext.SetVariableLocal("numberOfInstances", numberOfInstances);
            executionContext.SetVariableLocal("numberOfCompletedInstances", numberOfCompletedInstances);
            executionContext.SetVariableLocal("numberOfActiveInstances", numberOfActiveInstances);

            var childToken = token.CreateToken();
            childToken.Node = node;

            var loopCounter = 0;

            var childExecutionContext = ExecutionContext.Create(context, childToken);
            childExecutionContext.SetVariableLocal("loopCounter", loopCounter);

            //save changes.
            await childExecutionContext.Context.DbSession.FlushAsync();

            await this.ExecuteOriginalBehaviorAsync(childExecutionContext, loopCounter);

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
            return numberOfInstances;
        }

        public override async System.Threading.Tasks.Task LeaveAsync(ExecutionContext executionContext)
        {
            //设置活动实例为结束状态
            var context = executionContext.Context;
            var token = executionContext.Token;

            var parentToken = token.Parent;
            if (parentToken == null || !parentToken.IsMIRoot)
                throw new RuntimeException("Invalid multiInstance execution.");

            //fire inner activityEndEvent.
            var engine = executionContext.Engine;
            await engine.ProcessEventListener.ActivityEndAsync(executionContext);

            //Get parent-context.
            var parentExecution = ExecutionContext.Create(context, parentToken);

            var numberOfInstances = parentExecution.GetVariableLocal<int>("numberOfInstances");
            var numberOfCompletedInstances = parentExecution.GetVariableLocal<int>("numberOfCompletedInstances") + 1;
            var numberOfActiveInstances = parentExecution.GetVariableLocal<int>("numberOfActiveInstances");

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
                parentExecution.SetVariableLocal("numberOfActiveInstances", 0);

                //Remove token.
                token.Remove();

                //exit multi-instance loop activity.
                //parentToken.IsMIRoot = false;
                parentToken.Activate();

                await executionContext.Context.DbSession.FlushAsync();

                //leave without check loop.
                await base.LeaveAsync(parentExecution);
            }
            else
            {
                //Reset token data.
                //token.Clear();
                token.IsMIRoot = false;
                token.IsScope = false;
                token.TransitionId = null;
                token.ActivityInstance = null;

                executionContext.SetVariableLocal("loopCounter", loopCounter);
                await this.ExecuteOriginalBehaviorAsync(executionContext, loopCounter);
            }
        }
    }
}
