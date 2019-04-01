using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    public class ExecutionContext
    {
        private readonly Token token;
        private BpmnActivity transitionSource;
        private BpmnTransition transition;

        public ExecutionContext(Token token)
        {
            this.token = token;
        }

        public virtual Token Token => this.token;

        public virtual BpmnActivity Activity => this.token.Activity;

        /// <summary>
        /// Gets the current task instance.
        /// </summary>
        public virtual TaskInstance TaskInstance
        {
            get;
        }

        public BpmnTransition Transition { get => transition; set => transition = value; }

        public BpmnActivity TransitionSource { get => transitionSource; set => transitionSource = value; }

        public virtual void LeaveNode(string outgoing = null)
        {
            var transition = this.token.Activity.GetOutgoingById(outgoing);
            this.Activity.Leave(this, transition);
        }

        public virtual object GetVariable(string name)
        {
            VariableInstance varInst = null;

            var t = this.token;
            while (true)
            {
                varInst = t.ActivityInstance.GetVariableInstance(name);
                if (varInst != null)
                    break;

                t = t.Parent;
                if (t == null)
                    break;
            }

            var p = token.ProcessInstance;
            varInst = p.GetVariableInstance(name);

            return varInst?.GetValue();
        }

        public virtual TValue GetVariable<TValue>(string name)
        {
            var value = this.GetVariable(name);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }

        public virtual void SetVariable(string name, object value)
        {

        }

        protected virtual void OnEnterNode()
        {

        }

        protected virtual void OnLeaveNode()
        {

        }

        public virtual IExpressionEvaluator CreateExpressionEvaluator(string language = null)
        {
            return null;
        }
    }

    public interface IExpressionEvaluator
    {
        TResult Evaluate<TResult>(string expression);
    }
}
