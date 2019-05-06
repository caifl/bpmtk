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

        
        protected override int CreateInstances(ExecutionContext executionContext)
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

            token.Inactivate();

            var node = executionContext.Node as Activity;
            int numberOfActiveInstances = 1;
            int numberOfCompletedInstances = 0;

            executionContext.SetVariableLocal("numberOfInstances", numberOfInstances);
            executionContext.SetVariableLocal("numberOfCompletedInstances", numberOfCompletedInstances);
            executionContext.SetVariableLocal("numberOfActiveInstances", numberOfActiveInstances);

            var loopCounter = 0;
            var innerExecutionContext = executionContext.CreateInnerActivityContexts(1)[0];
            innerExecutionContext.SetVariableLocal("loopCounter", loopCounter);

            this.ExecuteOriginalBehavior(innerExecutionContext, loopCounter);

            return numberOfInstances;
        }

        public override void Leave(ExecutionContext executionContext)
        {
            var context = executionContext.Context;
            var token = executionContext.Token;

            var parentToken = token.Parent;
            if (parentToken == null || !parentToken.IsMIRoot)
                throw new RuntimeException("Invalid multiInstance execution.");

            //fire inner activityEndEvent.
            var eventListener = context.RuntimeManager.GetCompositeProcessEventListener();
            eventListener.ActivityEnd(executionContext);

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
                token.Inactivate();
                //token.IsEnded = true;
                token.Remove();

                //exit multi-instance loop activity.
                parentToken.Activate();

                executionContext.Flush();

                //leave without check loop.
                base.Leave(parentExecution);
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
                this.ExecuteOriginalBehavior(executionContext, loopCounter);
            }
        }
    }
}
