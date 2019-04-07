using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Bpmn2
{
    public class ParallelGateway : Gateway
    {

        protected override void OnActivate(ExecutionContext executionContext)
        {
            if (this.incomings.Count > 1)
            {
                //Waiting..
                return;
            }

            base.OnActivate(executionContext);
        }

        protected override ActivityInstance CreateActivityInstance(ExecutionContext executionContext)
        {
            if (this.incomings.Count > 1)
            {
                var token = executionContext.Token;
                token.Inactivate();

                var scopeToken = token.ResolveScope();
                var joinedTokens = scopeToken.GetInactiveTokensAt(token.Node);

                if(joinedTokens.Count == 1)
                    return base.CreateActivityInstance(executionContext);

                if(joinedTokens.Count > 1)
                {
                    var act = joinedTokens.Where(x => x.ActivityInstance != null)
                        .Select(x => x.ActivityInstance)
                        .FirstOrDefault();

                    return act;
                }
            }

            return base.CreateActivityInstance(executionContext);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            if(this.incomings.Count == 1)
            {
                base.Leave(executionContext, true);
                return;
            }

            var token = executionContext.Token;
            var scopeToken = token.ResolveScope();
            var inactiveTokens = scopeToken.GetInactiveTokensAt(token.Node);
            if(inactiveTokens.Count >= this.incomings.Count)
            {
                var context = executionContext.Context;

                //保留当前token.
                inactiveTokens.Remove(token);

                //保留rootToken.
                inactiveTokens.Remove(scopeToken);

                //删除其他完成的分支
                Token current = null;
                foreach (var pToken in inactiveTokens)
                {
                    current = pToken;
                    current.Remove(context);

                    //往上遍历
                    current = current.Parent;
                    while (current.Parent != null
                        && current.Parent.Children.Count == 1)
                    {
                        current.Remove(context);
                        current = current.Parent;
                    }
                }

                var parentToken = token.Parent;

                //尝试删除当前分支
                current = token;
                while (current.Parent != null
                    && current.Parent.Children.Count == 1)
                {
                    current.Remove(context);
                    current = current.Parent;
                }

                if (!current.Equals(token))
                    executionContext.ReplaceToken(current);

                //Activate activity-instance.
                base.OnActivate(executionContext);

                //leave
                base.Leave(executionContext, true);
            }
            else
            {
                //waiting..
            }
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        
    }
}
