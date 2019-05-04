using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public abstract class FlowNodeActivityBehavior : IFlowNodeActivityBehavior
    {
        public virtual System.Threading.Tasks.Task<bool> EvaluatePreConditionsAsync(ExecutionContext executionContext)
            => System.Threading.Tasks.Task.FromResult(true);

        protected virtual SequenceFlow GetDefaultOutgoing(ExecutionContext executionContext)
        {
            return null;
        }

        //protected virtual void ExecuteEventHandlers(string eventName, ExecutionContext executionContext)
        //{
        //    var node = executionContext.Node;
        //    var scripts = node.Scripts;

        //    if (scripts.Count > 0)
        //    {
        //        var list = scripts.Where(x => x.On.Equals(eventName)).ToList();
        //        foreach (var item in list)
        //            executionContext.ExecutScript(item.Text, item.ScriptFormat);
        //    }
        //}

        public virtual System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            return this.LeaveAsync(executionContext);
        }

        public virtual async System.Threading.Tasks.Task LeaveAsync(ExecutionContext executionContext)
        {
            await this.LeaveDefaultAsync(executionContext);
        }

        public virtual async System.Threading.Tasks.Task LeaveAsync(ExecutionContext executionContext, bool ignoreConditions)
        {
            var node = executionContext.Node;
            var outgoings = node.Outgoings;

            if (outgoings.Count == 0)
            {
                await executionContext.EndAsync();
                return;
            }

            IList<SequenceFlow> transitions;
            if (!ignoreConditions)
            {
                transitions = new List<SequenceFlow>();

                var evaluator = executionContext.GetEvaluator();

                foreach (var outgoing in outgoings)
                {
                    var condition = outgoing.ConditionExpression;
                    if (condition == null || string.IsNullOrEmpty(condition.Text)
                        || !evaluator.Evaluate<bool>(condition.Text))
                        continue;

                    transitions.Add(outgoing);
                }

                if (transitions.Count == 0)
                {
                    var deaultOutgoing = this.GetDefaultOutgoing(executionContext);
                    if (deaultOutgoing != null)
                        transitions.Add(deaultOutgoing);
                }

                if (transitions.Count == 0)
                    throw new RuntimeException("没有满足条件的分支可走");
            }
            else
                transitions = new List<SequenceFlow>(outgoings);

            await executionContext.LeaveNodeAsync(transitions);
        }

        private async System.Threading.Tasks.Task LeaveDefaultAsync(ExecutionContext executionContext)
        {
            var node = executionContext.Node;
            var outgoings = node.Outgoings;

            if (outgoings.Count == 0)
            {
                await executionContext.EndAsync();
                return;
            }

            SequenceFlow transition = null;
            if (outgoings.Count == 1)
                transition = outgoings[0];
            else
            {
                var evaluator = executionContext.GetEvaluator();
                foreach (var outgoing in outgoings)
                {
                    var condition = outgoing.ConditionExpression;
                    if (condition == null || string.IsNullOrEmpty(condition.Text)
                        || !evaluator.Evaluate<bool>(condition.Text))
                        continue;

                    transition = outgoing;
                    break;
                }

                if (transition == null)
                    transition = this.GetDefaultOutgoing(executionContext);
            }

            if (transition == null)
                throw new RuntimeException("没有满足条件的分支可走");

            await executionContext.LeaveNodeAsync(transition);
        }
    }
}
