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
        private Token token;
        private FlowNode transitionSource;
        private SequenceFlow transition;

        protected ExecutionContext(IContext context, Token token)
        {
            this.Context = context;
            this.token = token;
        }

        public static ExecutionContext Create(IContext context, Token token)
        {
            return new ExecutionContext(context, token);
        }

        public virtual ProcessInstance ProcessInstance => this.token.ProcessInstance;

        public virtual Token Token => this.token;

        public virtual FlowNode Node => this.token.Node;

        public virtual void ReplaceToken(Token token)
        {
            var oldToken = this.token;

            this.token = token;
            this.token.Node = oldToken.Node;
            this.token.Scope = oldToken.Scope;
            this.token.ActivityInstance = oldToken.ActivityInstance;

            //re-activate.
            this.token.Activate();
        }

        public virtual SequenceFlow Transition { get => transition; set => transition = value; }

        public virtual FlowNode TransitionSource { get => transitionSource; set => transitionSource = value; }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.token.ActivityInstance;
            set => this.token.ActivityInstance = value;
        }

        public virtual void LeaveNode(string outgoing = null)
        {
            this.Node.Leave(this);
        }

        public virtual object GetVariable(string name)
        {
            ExecutionObject execution = this.ActivityInstance;
            if (execution != null)
                return execution.GetVariable(name);

            execution = this.Scope;
            if (execution != null)
                return execution.GetVariable(name);

            return this.ProcessInstance.GetVariable(name);

            //VariableInstance varInst = null;

            //var t = this.token;
            //while (true)
            //{
            //    varInst = t.ActivityInstance.GetVariableInstance(name);
            //    if (varInst != null)
            //        break;

            //    t = t.Parent;
            //    if (t == null)
            //        break;
            //}

            //var p = token.ProcessInstance;
            //varInst = p.GetVariableInstance(name);

            //return varInst?.GetValue();
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
            ExecutionObject execution = this.ActivityInstance;
            if (execution != null)
            {
                execution.SetVariable(name, value);
                return;
            }

            execution = this.Scope;
            if (execution != null)
            {
                execution.SetVariable(name, value);
                return;
            }

            this.ProcessInstance.SetVariable(name, value);
        }

        public virtual IEvaluationContext CreateEvaluationContext()
        {
            return null;
        }

        public virtual IExpressionEvaluator CreateExpressionEvaluator(string language = null)
        {
            return null;
        }

        public virtual ActivityInstance Scope
        {
            get => this.token.Scope;
            set => this.token.Scope = value;
        }

        /// <summary>
        /// Gets or sets sub-process-instance.
        /// </summary>
        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual IContext Context
        {
            get;
        }
    }
}
