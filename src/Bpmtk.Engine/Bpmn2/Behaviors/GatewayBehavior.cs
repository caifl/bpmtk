using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    abstract class GatewayBehavior : FlowNodeBehavior
    {


    }

    class ParallelGatewayBehavior : GatewayBehavior
    {
        public ParallelGatewayBehavior()
        {

        }

        public override void Execute(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            var activity = token.Activity;

            var outgoings = activity.Outgoings;

            var list = new List<KeyValuePair<Token, BpmnTransition>>();

            foreach (var item in outgoings)
            {
                var fork = token.CreateToken();
        
                list.Add(new KeyValuePair<Token, BpmnTransition>(fork, item));
            }

            foreach (var entry in list)
            {
                var fork = entry.Key;
                var outgoing = entry.Value;

                outgoing.Take(new ExecutionContext(fork));
            }
        }
    }
}
