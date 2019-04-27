using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Models
{
    public class Token
    {
        private bool isInitialized;
        private FlowNode node;
        private ICollection<Token> children;
        private ICollection<Variable> variables;
        private IDictionary<string, Variable> variableByName;

        protected Token()
        {
            isInitialized = false;
        }

        public virtual bool IsEnded
        {
            get;
        }

        protected Token(Token parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.isInitialized = true;
            this.Parent = parent;
            this.children = new List<Token>();
            this.variables = new List<Variable>();
            this.variableByName = new Dictionary<string, Variable>();
            this.ProcessInstance = parent.ProcessInstance;
            this.IsActive = true;
        }

        public Token(ProcessInstance processInstance)
        {
            this.ProcessInstance = processInstance;
            this.variables = new List<Variable>();
            this.children = new List<Token>();
            this.IsActive = true;
        }

        public Token(ProcessInstance processInstance, FlowNode initialNode)
        {
            this.variables = new List<Variable>();
            this.variableByName = new Dictionary<string, Variable>();
            this.children = new List<Token>();
            this.ProcessInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance));
            this.node = initialNode ?? throw new ArgumentNullException(nameof(initialNode));
            this.IsActive = true;
            this.IsMIRoot = false;
            this.IsSuspended = false;

            this.OnNodeChanged();
        }

        public virtual long Id
        {
            get;
            protected set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual Token Parent
        {
            get;
            protected set;
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
                this.Parent.children.Remove(this);
            else
                this.ProcessInstance.Tokens.Remove(this);
        }

        //public virtual void Remove(IContext context)
        //{
        //    if (this.Parent != null)
        //    {
        //        this.Parent.children.Remove(this);
        //        var store = context.GetService<IInstanceStore>();
        //        store.Remove(this);
        //    }
        //    else
        //        throw new Exception("Can't delete root-token.");
        //}

        /// <summary>
        /// Gets the root-token.
        /// </summary>
        public virtual Token GetRoot()
            => this;

        protected void CollectInactiveTokensAt(FlowNode node, IList<Token> tokens)
        {
            if (!this.IsActive && node.Id.Equals(this.node.Id))
                tokens.Add(this);

            var children = this.children;
            foreach (var child in children)
                child.CollectInactiveTokensAt(node, tokens);
        }

        protected void CollectActiveTokens(IList<Token> tokens)
        {
            if (this.IsActive)
                tokens.Add(this);

            var children = this.children;
            foreach (var child in children)
                child.CollectActiveTokens(tokens);
        }

        public virtual IList<Token> GetInactiveTokensAt(FlowNode node)
        {
            var list = new List<Token>();
            this.CollectInactiveTokensAt(node, list);

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

        protected virtual void OnNodeChanged()
        {
            this.ActivityId = this.node?.Id;
        }

        protected virtual void EnsureInitialized()
        {
            if (!this.isInitialized)
            {
                if (this.node == null && this.ActivityId != null)
                {
                    //var procDef = this.ProcessInstance.ProcessDefinition;
                    //var deploymentId = procDef.DeploymentId;
                    //var dm = Context.Current.GetService<IDeploymentManager>();
                    //var model = dm.GetBpmnModel(deploymentId);
                    //this.node = model?.GetFlowElement(this.ActivityId) as FlowNode;
                }

                this.isInitialized = true;
            }
        }

        public virtual FlowNode Node
        {
            get
            {
                this.EnsureInitialized();

                return this.node;
            }
            set
            {
                this.node = value;
                this.ActivityId = value?.Id;
            }
        }

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
            this.children.Add(token);

            //var store = context.GetService<IInstanceStore>();
            //store.Add(token);

            return token;
        }

        protected virtual void EnsureVariablesInitialized()
        {
            if (this.variableByName == null)
                this.variableByName = this.variables.ToDictionary(x => x.Name);
        }

        public virtual object GetVariableLocal(string name)
        {
            this.EnsureVariablesInitialized();

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
                return variable.GetValue();

            return null;
        }

        public virtual IVariable GetVariable(string name)
        {
            if(this.variableByName == null)
                this.variableByName = this.variables.ToDictionary(x => x.Name);

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
                return variable;

            if (this.Parent != null)
                return this.Parent.GetVariable(name);

            return this.ProcessInstance.GetVariable(name);
        }

        public virtual void SetVariable(string name, object value)
        {
            if (this.variableByName == null)
                this.variableByName = this.variables.ToDictionary(x => x.Name);

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
                variable.SetValue(value);

            if (this.Parent != null)
                this.Parent.SetVariable(name, value);

            this.ProcessInstance.SetVariable(name, value);
        }

        public virtual void SetVariableLocal(string name, object value)
        {
            this.EnsureVariablesInitialized();

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
            {
                variable.SetValue(value);
                return;
            }

            //variable = new Variable(this, name, value);
            //this.variableByName.Add(name, variable);
            //this.variables.Add(variable);

            //Try update historic data.
            if (this.ActivityInstance != null)
                this.ActivityInstance.SetVariable(name, value);
        }

        public virtual Token ResolveScope()
        {
            //if (this.Scope == null)
            //    return this;

            //var activityInstanceId = this.Scope.Id;
            //var p = this.Parent;
            //while(p != null && p.ActivityInstance != null)
            //{
            //    if(activityInstanceId == p.ActivityInstance.Id)
            //        return p;

            //    p = p.Parent;
            //}

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
            protected set;
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
