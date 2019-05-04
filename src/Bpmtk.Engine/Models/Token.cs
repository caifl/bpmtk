using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Models
{
    public class Token
    {
        protected FlowNode node;
        private IDictionary<string, Variable> variableByName;

        protected Token()
        { }

        public virtual bool IsEnded
        {
            get;
            set;
        }

        protected Token(Token parent) 
            : this(parent.ProcessInstance)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.Parent = parent;
        }

        public Token(ProcessInstance processInstance)
        {
            this.ProcessInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance));
            this.IsActive = true;

            this.Children = new List<Token>();
            this.Variables = new List<Variable>();
            this.variableByName = new Dictionary<string, Variable>();
        }

        public virtual long Id
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            set;
        }

        public virtual Token Parent
        {
            get;
            set;
        }

        public virtual FlowNode Node
        {
            get => this.node;
            set
            {
                this.node = value;
                this.ActivityId = value?.Id;
            }
        }

        //public virtual DateTime EnterTime
        //{
        //    get;
        //    set;
        //}

        //public virtual DateTime LeaveTime
        //{
        //    get;
        //    set;
        //}
        public virtual bool IsScope
        {
            get;
            set;
        }

        public virtual void Activate()
        {
            this.IsActive = true;
        }

        public virtual void Inactivate()
        {
            this.IsActive = false;
        }

        public virtual void Remove()
        {
            if (this.Parent != null)
                this.Parent.Children.Remove(this);
            else
                this.ProcessInstance.Tokens.Remove(this);
        }

        protected void CollectInactiveTokensAt(string activityId, IList<Token> tokens)
        {
            if (!this.IsActive && activityId.Equals(this.ActivityId))
                tokens.Add(this);

            var children = this.Children;
            foreach (var child in children)
                child.CollectInactiveTokensAt(activityId, tokens);
        }

        protected void CollectActiveTokens(IList<Token> tokens)
        {
            if (this.IsActive)
                tokens.Add(this);

            var children = this.Children;
            foreach (var child in children)
                child.CollectActiveTokens(tokens);
        }

        public virtual IList<Token> GetInactiveTokensAt(string activityId)
        {
            var list = new List<Token>();
            this.CollectInactiveTokensAt(activityId, list);

            return list;
        }

        public virtual IList<Token> Children
        {
            get;
            set;
        }

        public virtual ICollection<Variable> Variables
        {
            get;
            set;
        }

        /// <summary>
        /// Reset context.
        /// </summary>
        public virtual void Clear()
        {
            this.variableByName.Clear();
            this.Variables.Clear();
        }

        //protected virtual void OnNodeChanged()
        //{
        //    this.ActivityId = this.node?.Id;
        //}

        protected virtual void EnsureVariablesInitialized()
        {
            if (this.variableByName != null)
                return;

            if (this.Variables != null)
                this.variableByName = this.Variables.ToDictionary(x => x.Name);
        }

        //public virtual FlowNode Node
        //{
        //    get
        //    {
        //        return this.node;
        //    }
        //    set
        //    {
        //        this.node = value;
        //        this.ActivityId = value?.Id;
        //    }
        //}

        public virtual bool IsActive
        {
            get;
            set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual string TransitionId
        {
            get;
            set;
        }

        //public virtual ProcessInstance CreateSubProcessInstance(IContext context)
        //{
        //    var procInst = new ProcessInstance(null, null);

        //    return procInst;
        //}

        public virtual Token CreateToken()
        {
            var token = new Token(this);
            this.Children.Add(token);

            return token;
        }

        /// <summary>
        /// Find variable instance by name in local scope.
        /// </summary>
        public virtual Variable FindVariableByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.EnsureVariablesInitialized();

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
                return variable;

            return null;
        }

        public virtual bool TryGetVariable(string name, out object value)
        {
            value = null;

            Variable variable = this.FindVariableByName(name);
            if(variable != null)
            {
                value = variable.GetValue();
                return true;
            }

            if (this.Parent != null)
                return this.Parent.TryGetVariable(name, out value);

            variable = this.ProcessInstance.FindVariableByName(name);
            if(variable != null)
            {
                value = variable.GetValue();
                return true;
            }

            return false;
        }

        public virtual object GetVariableLocal(string name)
            => this.FindVariableByName(name)?.GetValue();

        public virtual object GetVariable(string name)
        {
            object value = null;
            if (this.TryGetVariable(name, out value))
                return value;

            return null;
        }

        public virtual void SetVariableLocal(string name, object value)
        {
            Variable variable = this.FindVariableByName(name);
            if (variable != null)
            {
                variable.SetValue(value);
            }
            else
            {
                //add new variable object.
                this.AddVariable(name, value);
            }

            //Update historic-variable.
            if (this.ActivityInstance != null)
                this.ActivityInstance.SetVariable(name, value);
        }

        public virtual void SetVariable(string name, object value)
        {
            this.EnsureVariablesInitialized();

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
            {
                variable.SetValue(value);
                return;
            }

            if (this.Parent != null)
            {
                this.Parent.SetVariable(name, value);
                return;
            }

            this.ProcessInstance.SetVariable(name, value);
        }

        protected virtual Variable AddVariable(string name, object value,
            IVariableType type = null)
        {
            if (value == null)
                return null;

            if (type == null)
                type = VariableType.Resolve(value);

            var variable = new Variable();
            variable.Name = name;
            variable.Type = type.Name;

            type.SetValue(variable, value);

            this.Variables.Add(variable);
            this.variableByName.Add(name, variable);

            return variable;
        }

        public virtual Token ResolveScope()
        {
            var parent = this.Parent;

            while (parent != null)
            {
                if (parent.IsScope && !parent.IsMIRoot)
                {
                    return parent;
                }

                parent = parent.Parent;
            }

            return null;
        }

        public virtual IList<Token> GetActiveTokens()
        {
            var list = new List<Token>();
            this.CollectActiveTokens(list);

            return list;
        }

        //public virtual void End(IContext context,
        //    bool isImplicit = false, string endReason = null)
        //{
        //    
        //}

        /// <summary>
        /// Sends a signal to this token. leaves the current {@link #getNode() node} over the default transition
        /// </summary>
        //public virtual void Signal(IContext context)
        //{
        //    if (this.node == null)
        //        throw new RuntimeException(this + " is not positioned in a node");

        //    //var defaultTransition = node.getDefaultLeavingTransition();
        //    //if (defaultTransition == null)
        //    //{
        //    //    throw new JbpmException(node + " has no default transition");
        //    //}

        //    this.node.Signal(ExecutionContext.Create(context, this), null, null);
        //}

        //public virtual void Signal(IContext context,
        //    string signalName, 
        //    IDictionary<string, object> signalData)
        //{
        //    this.node.Signal(ExecutionContext.Create(context, this), signalName, signalData);
        //}

        public virtual bool IsSuspended
        {
            get;
            set;
        }

        public virtual bool IsMIRoot
        {
            get;
            set;
        }

        //public virtual void Terminate(IContext context,
        //    string endReason = null)
        //{
        //    var tokens = this.children.ToList();

        //    foreach (var child in tokens)
        //    {
        //        child.Terminate(context, endReason);
        //        this.children.Remove(child);
        //    }

        //    this.Inactivate();

        //    if (this.ActivityInstance != null)
        //        this.ActivityInstance.Terminate(context, endReason);
        //}

        //public virtual void Resume(IContext context)
        //{
        //    this.IsSuspended = false;
        //}

        //public virtual void Suspend(IContext context)
        //{
        //    this.IsSuspended = false;
        //}
    }
}
