using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class ParallelGatewayActivityBehavior : GatewayActivityBehavior
    {
        protected override SequenceFlow GetDefaultOutgoing(ExecutionContext executionContext)
            => null;

        public override System.Threading.Tasks.Task<bool> EvaluatePreConditionsAsync(ExecutionContext executionContext)
        {
            var node = executionContext.Node;
            if (node.Incomings.Count <= 1)
                return System.Threading.Tasks.Task.FromResult(true);

            var token = executionContext.Token;
            token.Inactivate();

            //find inactive-tokens in closest scope.
            var processInstance = token.ProcessInstance;
            var scope = token.ResolveScope();
            IList<Token> joinedTokens = null;
            var activityId = node.Id;
            if (scope != null)
                 joinedTokens = scope.GetInactiveTokensAt(activityId);
            else
                joinedTokens = processInstance.GetInactiveTokensAt(activityId);

            
            //Ensure every incoming transitions have at least one token.
            //tokens.Select(x => x.TransitionId).Distinct().Count();
            var count = joinedTokens.Count;

            //remove current token from list.
            joinedTokens.Remove(token); 

            //Store joined-tokens into current context.
            executionContext.JoinedTokens = joinedTokens;

            var expectedTokenCount = node.Incomings.Count;
            var result = count >= expectedTokenCount;
            
            var logger = executionContext.Logger;
            logger.LogInformation($"There are '{count} of {expectedTokenCount}' tokens joined at parallelGateway '{activityId}'.");

            return System.Threading.Tasks.Task.FromResult(result);
        }

        public override async System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            //Join tokens.
            var joinedTokens = executionContext.JoinedTokens;
            if (joinedTokens != null && joinedTokens.Count > 0)
                await executionContext.JoinAsync();

            var node = executionContext.Node;
            if (node.Outgoings.Count <= 1)
            {
                await base.ExecuteAsync(executionContext);
                return;
            }

            //leave and ignore outgoing conditions.
            await base.LeaveAsync(executionContext, true);
        }
    }
}
