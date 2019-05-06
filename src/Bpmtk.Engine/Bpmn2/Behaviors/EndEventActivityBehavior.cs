using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class EndEventActivityBehavior : FlowNodeActivityBehavior
    {
        public override void Execute(ExecutionContext executionContext)
        {
            var context = executionContext.Context;
            var endEvent = executionContext.Node as EndEvent;

            var list = new List<EventDefinition>(endEvent.EventDefinitions);
            list.AddRange(endEvent.EventDefinitionRefs);

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item is ErrorEventDefinition)
                    {
                        var errorEvent = (item as ErrorEventDefinition).ErrorRef;

                        break;
                    }

                    if (item is TerminateEventDefinition)
                    {
                        var terminateEvent = item as TerminateEventDefinition;
                        executionContext.End(); // context, false);
                        return;
                    }
                }
            }

            executionContext.End(); //context, true);
        }
    }
}
