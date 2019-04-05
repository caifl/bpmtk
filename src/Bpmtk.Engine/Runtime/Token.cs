using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Stores;
using System.Collections.ObjectModel;

namespace Bpmtk.Engine.Runtime
{
    public class Token
    {
        private Token parent;
        private ProcessInstance processInstance;
        private ActivityInstance activityInstance;
        private FlowNode node;
        private ICollection<Token> children;

        protected Token()
        {}

        protected Token(Token parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.parent = parent;
            this.children = new List<Token>();
            this.processInstance = parent.ProcessInstance;
            this.IsActive = true;
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
            get => this.parent;
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
            if (this.parent != null)
                this.parent.children.Remove(this);
            else
                throw new Exception("Can't delete root-token.");
        }

        /// <summary>
        /// Gets the root-token.
        /// </summary>
        public virtual Token GetRoot()
            => this.processInstance.Token;

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
            get => new List<Token>(this.children);
        }

        public Token(ProcessInstance processInstance, FlowNode initialNode)
        {
            this.children = new List<Token>();
            this.processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance));
            this.node = initialNode ?? throw new ArgumentNullException(nameof(initialNode));
            this.IsActive = true;
            this.IsLoopActivity = false;
            this.IsSuspended = false;

            this.OnNodeChanged();

            //this.ActivateActivity();
        }

        protected virtual void OnNodeChanged()
        {
            this.ActivityId = this.node?.Id;
        }

        protected virtual void ActivateActivity()
        {
            this.activityInstance = new ActivityInstance(this);
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

        public virtual bool IsActive
        {
            get;
            protected set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get => this.activityInstance;
        }

        public virtual ProcessInstance ProcessInstance { get => processInstance; }

        public virtual Token CreateToken()
        {
            var token = new Token(this);
            this.children.Add(token);

            var store = Context.Current.GetService<IProcessInstanceStore>();
            store.Add(token);

            return token;
        }

        //protected virtual void ExecuteLoopActivity(StandardLoopCharacteristics loopCharacteristics)
        //{

        //}

        //protected virtual void ExecuteMultiInstanceActivity(MultiInstanceLoopCharacteristics loopCharacteristics)
        //{

        //}

        //protected virtual void ExecuteSubProcess()
        //{

        //}

        protected IList<Token> GetJoinedTokens()
        {
            return null;
        }

        //protected virtual bool ExecuteParallelGateway()
        //{
        //    this.isActive = false;

        //    var joinedTokens = this.GetJoinedTokens();
        //    if(joinedTokens.Count >= this.activiy.Incomings.Count)
        //    {
        //        //Continue ..
        //        var act = joinedTokens.Where(x => x.activityInstance != null)
        //            .Select(x => x.activityInstance)
        //            .FirstOrDefault();

        //        if (act == null)
        //            act = new ActivityInstance();

        //        this.ActivateNode();

        //        //Fork
        //        var list = new List<Token>();

        //        foreach (var outgoing in this.activiy.Outgoings)
        //            list.Add(this.CreateToken());

        //        foreach (var t in list)
        //            t.Take(null);

        //        return true;
        //    }
        //    else
        //    {
        //        //Waiting tokens.
        //        return false;
        //    }
        //}

        //protected virtual void Take(SequenceFlow transition)
        //{

        //}

        //public virtual Token CreateToken()
        //{
        //    return null;
        //}

        //protected virtual void ActivateNode()
        //{
        //    this.activityInstance.Activate();
        //}

        //public virtual void EnterNode(FlowNode node)
        //{
        //    this.activiy = node;
        //    //this.isActive = true;

        //    if (node is ParallelGateway)
        //    {
        //        this.ExecuteParallelGateway();
        //        return;
        //    }

        //    if (node is Activity && ((Activity)node).LoopCharacteristics != null)
        //    {
        //        var activity = node as Activity;
        //        var loopCharacteristics = activity.LoopCharacteristics;
        //        this.isLoopActivity = true;

        //        if (loopCharacteristics is MultiInstanceLoopCharacteristics)
        //        {
        //            var multiInstance = loopCharacteristics as MultiInstanceLoopCharacteristics;
        //            this.ExecuteMultiInstanceActivity(multiInstance);
        //        }
        //        else
        //        {
        //            var loop = loopCharacteristics as StandardLoopCharacteristics;
        //            this.ExecuteLoopActivity(loop);
        //        }

        //        return;
        //    }

        //    if(node is SubProcess)
        //    {
        //        this.ExecuteSubProcess();
        //        return;
        //    }

        //    if(node is ScriptTask)
        //    {
        //        this.ExecuteScriptTask();
        //        return;
        //    }

        //    var context = new ExecutionContext(this);
            
        //}
        public virtual IList<Token> GetActiveTokens()
        {
            var list = new List<Token>();
            this.CollectActiveTokens(list);

            return list;
        }

        public virtual void End(bool isImplicit = false,
            string endReason = null)
        {
            this.Inactivate();

            var store = Context.Current.GetService<IProcessInstanceStore>();
            store.Add(new HistoricToken(new ExecutionContext(this), "end"));

            if (this.parent != null)
            {
                var parentToken = this.parent;

                //判断是否在子流程中
                var container = this.node.Container;
                if (container is SubProcess)
                {
                    this.Remove();

                    if (parentToken.children.Count > 0)
                        return;

                    var subProcess = container as SubProcess;
                    subProcess.Leave(new ExecutionContext(parentToken));
                    return;
                }
            }

            //结束流程实例
            this.processInstance.End(isImplicit, endReason);
        }

        /// <summary>
        /// Sends a signal to this token. leaves the current {@link #getNode() node} over the default transition
        /// </summary>
        public virtual void Signal()
        {
            if (this.node == null)
                throw new RuntimeException(this + " is not positioned in a node");

            //var defaultTransition = node.getDefaultLeavingTransition();
            //if (defaultTransition == null)
            //{
            //    throw new JbpmException(node + " has no default transition");
            //}

            this.node.Signal(new ExecutionContext(this), null, null);
        }

        public virtual void Signal(string signalName, object signalData)
        {
            this.node.Leave(new ExecutionContext(this));
            //this.node.Signal(new ExecutionContext(this), signalName,
            //    signalData);
        }

        public virtual bool IsSuspended
        {
            get;
            protected set;
        }

        public virtual bool IsLoopActivity
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
