using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.Runtime
{
    public class Token
    {
        private bool isInitialized;
        private FlowNode node;
        private ICollection<Token> children;

        protected Token()
        {
            isInitialized = false;
        }

        protected Token(Token parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.isInitialized = true;
            this.Parent = parent;
            this.children = new List<Token>();
            this.ProcessInstance = parent.ProcessInstance;
            this.IsActive = true;
        }

        public Token(ProcessInstance processInstance, FlowNode initialNode)
        {
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

        public virtual void Activate()
        {
            this.IsActive = true;
        }

        public virtual void Inactivate()
        {
            this.IsActive = false;
        }

        public virtual void Remove(IContext context)
        {
            if (this.Parent != null)
            {
                this.Parent.children.Remove(this);
                var store = context.GetService<IInstanceStore>();
                store.Remove(this);
            }
            else
                throw new Exception("Can't delete root-token.");
        }

        /// <summary>
        /// Gets the root-token.
        /// </summary>
        public virtual Token GetRoot()
            => this.ProcessInstance.Token;

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

        public virtual IReadOnlyList<Token> Children
        {
            get => this.children.ToList();
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
                    var procDef = this.ProcessInstance.ProcessDefinition;
                    var deploymentId = procDef.DeploymentId;
                    var dm = Context.Current.GetService<IDeploymentManager>();
                    var model = dm.GetBpmnModel(deploymentId);
                    this.node = model?.GetFlowElement(this.ActivityId) as FlowNode;
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
            protected set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets SubProcess activity-instance.
        /// </summary>
        public virtual ActivityInstance Scope
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            protected set;
        }

        public virtual Token CreateToken(IContext context)
        {
            var token = new Token(this);
            this.children.Add(token);

            var store = context.GetService<IInstanceStore>();
            store.Add(token);

            return token;
        }

        //public virtual Token CreateToken()
        //{
        //    var token = new Token(this);
        //    this.children.Add(token);

        //    var store = Context.Current.GetService<IInstanceStore>();
        //    store.Add(token);

        //    return token;
        //}

        public virtual Token ResolveScope()
        {
            if (this.Scope == null)
                return this.ProcessInstance.Token;

            var activityInstanceId = this.Scope.Id;
            var p = this.Parent;
            while(p != null && p.ActivityInstance != null)
            {
                if(activityInstanceId == p.ActivityInstance.Id)
                    return p;

                p = p.Parent;
            }

            return this.ProcessInstance.Token;
        }

        public virtual IList<Token> GetActiveTokens()
        {
            var list = new List<Token>();
            this.CollectActiveTokens(list);

            return list;
        }

        public virtual void End(IContext context,
            bool isImplicit = false, string endReason = null)
        {
            //complete act-inst.
            if (this.ActivityInstance != null)
                this.ActivityInstance.Finish();

            this.Inactivate();

            var store = context.GetService<IInstanceStore>();
            store.Add(new HistoricToken(ExecutionContext.Create(context, this), "end"));

            if (this.Parent != null)
            {
                var parentToken = this.Parent;

                //判断是否在子流程中
                var container = this.node.Container;
                if (container is SubProcess)
                {
                    this.Remove(context);

                    if (parentToken.children.Count > 0)
                        return;

                    var subProcess = container as SubProcess;
                    subProcess.Leave(ExecutionContext.Create(context, parentToken));
                    return;
                }
            }

            //结束流程实例
            this.ProcessInstance.End(context, isImplicit, endReason);
        }

        /// <summary>
        /// Sends a signal to this token. leaves the current {@link #getNode() node} over the default transition
        /// </summary>
        public virtual void Signal(IContext context)
        {
            if (this.node == null)
                throw new RuntimeException(this + " is not positioned in a node");

            //var defaultTransition = node.getDefaultLeavingTransition();
            //if (defaultTransition == null)
            //{
            //    throw new JbpmException(node + " has no default transition");
            //}

            this.node.Signal(ExecutionContext.Create(context, this), null, null);
        }

        public virtual void Signal(IContext context,
            string signalName, 
            object signalData)
        {
            this.node.Signal(ExecutionContext.Create(context, this), signalName, signalData);
        }

        public virtual bool IsSuspended
        {
            get;
            protected set;
        }

        public virtual bool IsMIRoot
        {
            get;
            protected set;
        }

        public virtual void Resume()
        {
            this.IsSuspended = false;
        }

        public virtual void Suspend()
        {
            this.IsSuspended = false;
        }
    }
}
