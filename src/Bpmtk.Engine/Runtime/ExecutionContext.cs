using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Expressions;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    public class ExecutionContext
    {
        private readonly Token token;
        private FlowNode transitionSource;
        private SequenceFlow transition;

        public ExecutionContext(Token token)
        {
            this.token = token;
        }

        public virtual Token Token => this.token;

        public virtual FlowNode Node => this.token.Node;

        /// <summary>
        /// Gets the current task instance.
        /// </summary>
        public virtual TaskInstance TaskInstance
        {
            get;
        }

        public virtual SequenceFlow Transition { get => transition; set => transition = value; }

        public virtual FlowNode TransitionSource { get => transitionSource; set => transitionSource = value; }

        public virtual void LeaveNode(string outgoing = null)
        {
            //var transition = this.token.Activity.GetOutgoingById(outgoing);
            this.Node.Leave(this, outgoing);
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

        public virtual IEvaluationContext CreateEvaluationContext()
        {
            return null;
        }

        public virtual IExpressionEvaluator CreateExpressionEvaluator(string language = null)
        {
            return null;
        }

        /// <summary>
        /// Gets or sets sub-process-instance.
        /// </summary>
        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual IContext Context => Bpmtk.Engine.Context.Current;
    }

    
}
