using System;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class ParallelGatewayActivityBehavior : GatewayActivityBehavior
    {
        public override System.Threading.Tasks.Task<bool> CanActivateAsync(ExecutionContext executionContext,
            IList<Token> joinedTokens)
        {
            var node = executionContext.Node;
            if (node.Incomings.Count <= 1)
                return System.Threading.Tasks.Task.FromResult(true);

            var token = executionContext.Token;
            token.Inactivate();

            //find scope execution.
            //var scopeExecution = execution.ResolveScope();
            var tokens = executionContext.GetJoinedTokens();
            if(joinedTokens != null)
            {
                foreach (var t in tokens)
                    joinedTokens.Add(t);
            }

            //var items = scopeExecution(node);
            var count = tokens.Count;//tokens.Select(x => x.TransitionId).Distinct().Count();
            return System.Threading.Tasks.Task.FromResult(count >= node.Incomings.Count);
        }

        public override async System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            var node = executionContext.Node;
            if (node.Incomings.Count <= 1)
            {
                await base.ExecuteAsync(executionContext);
                return;
            }

            //find scope execution.
            var current = executionContext.Token;
            var tokens = executionContext.GetJoinedTokens();
            tokens.Remove(current);

            await base.LeaveAsync(executionContext, false, tokens);
        }
    }
}
