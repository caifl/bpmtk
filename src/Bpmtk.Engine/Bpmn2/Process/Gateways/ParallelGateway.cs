using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class ParallelGateway : Gateway
    {
        public override void Execute(ExecutionContext executionContext)
        {
            if(this.incomings.Count == 1)
            {
                base.Execute(executionContext);
                return;
            }

            var token = executionContext.Token;
            token.Inactivate();

            var rootToken = token.GetRoot();
            var inactiveTokens = rootToken.GetInactiveTokensAt(token.Node);
            if(inactiveTokens.Count >= this.incomings.Count)
            {
                //create activity-instance

                //保留当前token.
                inactiveTokens.Remove(token);

                //保留rootToken.
                inactiveTokens.Remove(rootToken);

                //var parents = new List<Token>();

                //删除其他完成的分支
                Token current = null;
                foreach (var pToken in inactiveTokens)
                {
                    current = pToken;
                    current.Remove();

                    //往上遍历
                    current = current.Parent;
                    while (current.Parent != null
                        && current.Parent.Children.Count == 1)
                    {
                        current.Remove();
                        current = current.Parent;
                    }
                    //while(current != null)
                    //{
                    //    current.Remove();

                    //    current = current.Parent;
                    //    if (current == null
                    //        || current.Equals(rootToken)
                    //        || current.Children.Count > 0)
                    //    {
                    //        break;
                    //    }
                    //}
                }

                var parentToken = token.Parent;

                //尝试删除当前分支
                current = token;
                while (current.Parent != null
                    && current.Parent.Children.Count == 1)
                {
                    current.Remove();
                    current = current.Parent;
                }

                if (!current.Equals(token))
                    executionContext.ReplaceToken(current);

                ////create activity-instance
                //inactiveTokens.Remove(token);

                //if(inactiveTokens.Count > 0)
                //{
                    
                //}

                //var parent = token.Parent;
                //if(parent != null && !parent.IsActive && parent.Children.Count == 1)
                //{
                //    token.Remove();
                //    executionContext.ReplaceToken(parent);
                //}

                base.Execute(executionContext);
            }
            else
            {
                //waiting..
            }
        }

        public override void Leave(ExecutionContext executionContext)
        {
            if (this.outgoings.Count <= 1)
            {
                base.Leave(executionContext);
                return;
            }

            //fork.
            var token = executionContext.Token;
            token.Inactivate();

            var list = new List<ParallelTransition>();
            foreach(var outgoing in this.outgoings)
            {
                var childToken = token.CreateToken();
                childToken.Node = this;
                list.Add(new ParallelTransition(childToken, outgoing));
            }

            foreach (var transition in list)
                transition.Take();
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        class ParallelTransition
        {
            private readonly Token token;
            private readonly SequenceFlow transition;

            public ParallelTransition(Token token, SequenceFlow transition)
            {
                this.token = token;
                this.transition = transition;
            }

            public virtual void Take()
            {
                var executionContext = new ExecutionContext(this.token);
                executionContext.TransitionSource = transition.SourceRef;
                executionContext.Transition = transition;

                transition.Take(executionContext);
            }
        }
    }
}
