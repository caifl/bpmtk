using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class ParallelMultiInstanceActivityBehavior : MultiInstanceActivityBehavior
    {
        public ParallelMultiInstanceActivityBehavior(ActivityBehavior innerActivityBehavior, MultiInstanceLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior, loopCharacteristics)
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
            int numberOfActiveInstances = numberOfInstances;
            int numberOfCompletedInstances = 0;

            executionContext.SetVariableLocal("numberOfInstances", numberOfInstances);
            executionContext.SetVariableLocal("numberOfCompletedInstances", numberOfCompletedInstances);
            executionContext.SetVariableLocal("numberOfActiveInstances", numberOfActiveInstances);

            var innerExecutions = executionContext.CreateInnerActivityContexts(numberOfInstances);            

            for(var i = 0; i < numberOfInstances; i ++)
            {
                var innerExecution = innerExecutions[i];
                var innerToken = innerExecution.Token;

                if (!innerToken.IsEnded && !token.IsEnded)
                {
                    innerExecution.SetVariableLocal("loopCounter", i);
                    this.ExecuteOriginalBehavior(innerExecution, i);
                }
            }

            return numberOfInstances;
        }

        public override void Leave(ExecutionContext executionContext)
        {
            var context = executionContext.Context;
            var token = executionContext.Token;

            var parentToken = token.Parent;
            if (parentToken == null || !parentToken.IsMIRoot)
                throw new RuntimeException("Invalid multiInstance execution.");

            //Get parent-context.
            var ecm = context.ExecutionContextManager;
            var parentContext = ecm.GetOrCreate(parentToken);

            var numberOfInstances = parentContext.GetVariableLocal<int>("numberOfInstances");
            var numberOfCompletedInstances = parentContext.GetVariableLocal<int>("numberOfCompletedInstances") + 1;
            var numberOfActiveInstances = parentContext.GetVariableLocal<int>("numberOfActiveInstances") - 1;

            parentContext.SetVariableLocal("numberOfCompletedInstances", numberOfCompletedInstances);
            parentContext.SetVariableLocal("numberOfActiveInstances", numberOfActiveInstances);
            
            token.Inactivate();

            //fire inner activityEndEvent.
            var runtimeManager = context.RuntimeManager;
            var eventListener = runtimeManager.GetCompositeProcessEventListener();
            eventListener.ActivityEnd(executionContext);

            var loopDataOutputRef = this.loopCharacteristics.LoopDataOutputRef;
            var outputDataItem = this.loopCharacteristics.OutputDataItem;

            //Update outputDataItem
            var loopOutputRef = loopDataOutputRef;
            if (loopOutputRef != null && outputDataItem != null)
            {
                var loopCounter = executionContext.GetVariableLocal<int>("loopCounter");

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

            if (numberOfCompletedInstances >= numberOfInstances || this.IsCompleted(parentContext))
            {
                if(numberOfActiveInstances > 0)
                    parentContext.SetVariableLocal("numberOfActiveInstances", 0);

                var tokens = new List<Token>(parentToken.Children);
                var date = Clock.Now;

                foreach(var innerToken in tokens)
                {
                    if (innerToken.IsActive)
                    {
                        var act = innerToken.ActivityInstance;

                        act.State = ExecutionState.Completed;
                        act.LastStateTime = date;

                        innerToken.Inactivate();
                    }
         
                    innerToken.Remove();
                }

                //exit multi-instance loop activity.
                parentToken.Activate();

                executionContext.Context.DbSession.Flush();

                //leave without check loop.
                base.Leave(parentContext);
            }
        }
    }
}
