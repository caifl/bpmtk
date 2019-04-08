using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Bpmn2
{
    public class ParallelGateway : Gateway
    {
        protected override void Activate(ExecutionContext executionContext)
        {
            if (this.incomings.Count > 1)
            {
                var token = executionContext.Token;

                token.Inactivate();

                var scopeToken = token.ResolveScope();
                var joinedTokens = scopeToken.GetInactiveTokensAt(token.Node);

                if (joinedTokens.Count == 1)
                {
                    token.ActivityInstance = ActivityInstance.Create(executionContext);
                    return;
                }

                if(joinedTokens.Count > 1)
                {
                    token.ActivityInstance = joinedTokens.Where(x => x.ActivityInstance != null)
                        .Select(x => x.ActivityInstance)
                        .FirstOrDefault();
                }

                if(joinedTokens.Count >= this.incomings.Count)
                {
                    //fire activity-instance activated event.
                    this.OnActivating(executionContext);

                    //Merge concurrent tokens.
                    this.Join(executionContext, scopeToken, joinedTokens);

                    //fire activity-instance activated event.
                    this.OnActivated(executionContext);

                    //leave all branches without check conditions.
                    base.Leave(executionContext, true);
                }
                else
                {
                    //waiting..
                }
            }
            else
                base.Activate(executionContext);
        }

        protected virtual void Join(ExecutionContext executionContext,
            Token scopeToken,
            IList<Token> inactiveTokens)
        {
            var token = executionContext.Token;
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
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
