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

            var inactiveTokens = token.GetRoot().GetInactiveTokensAt(token.Node);
            if(inactiveTokens.Count >= this.incomings.Count)
            {
                //create activity-instance
                inactiveTokens.Remove(token);

                if(inactiveTokens.Count > 0)
                {
                    foreach (var pToken in inactiveTokens)
                        pToken.Remove();
                }

                var parent = token.Parent;
                if(parent != null && !parent.IsActive && parent.Children.Count() == 1)
                {
                    token.Remove();
                    executionContext.ReplaceToken(parent);
                }

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
            var list = new List<ParallelTransition>();

            var concurrentRoot = token.Parent != null ? token.Parent : token;
            var index = 0;
            if (!token.Equals(concurrentRoot))
            {
                list.Add(new ParallelTransition(token, this.outgoings[0]));
                index = 1;
            }
            else
            {
                token.Inactivate();
            }

            for(var i = index; i < this.outgoings.Count; i ++)
            {
                var outgoing = this.outgoings[index];

                var childToken = concurrentRoot.CreateToken();
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
