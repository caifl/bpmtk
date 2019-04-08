using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class EndEvent : ThrowEvent
    {
        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            var context = executionContext.Context;

            var list = new List<EventDefinition>(this.EventDefinitions);
            list.AddRange(this.EventDefinitionRefs);

            if(list.Count > 0)
            {
                foreach(var item in list)
                {
                    if(item is ErrorEventDefinition)
                    {
                        var errorEvent = (item as ErrorEventDefinition).ErrorRef;

                        break;
                    }

                    if(item is TerminateEventDefinition)
                    {
                        var terminateEvent = item as TerminateEventDefinition;
                        token.End(context, false);
                        return;
                    }
                }
            }

            token.End(context, true);
        }
    }
}
