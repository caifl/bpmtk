using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    public class Execution
    {
        private bool isInitialized;
        private FlowNode node;
        private ICollection<Execution> children;
        private ICollection<Variable> variables;
        private IDictionary<string, Variable> variableByName;

        protected Execution()
        {
            isInitialized = false;
        }

        protected Execution(Execution parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            this.isInitialized = true;
            this.Parent = parent;
            this.children = new List<Execution>();
            this.variables = new List<Variable>();
            this.variableByName = new Dictionary<string, Variable>();
            this.ProcessInstance = parent.ProcessInstance;
            this.IsActive = true;
        }

        public Execution(ProcessInstance processInstance, FlowNode initialNode)
        {
            this.variables = new List<Variable>();
            this.variableByName = new Dictionary<string, Variable>();
            this.children = new List<Execution>();
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

        public virtual Execution Parent
        {
            get;
            protected set;
        }

        public virtual DateTime EnterTime
        {
            get;
            set;
        }

        public virtual DateTime LeaveTime
        {
            get;
            set;
        }

        public virtual IContext Context => this.context;

        public virtual void Activate()
        {
            this.IsActive = true;
        }

        public virtual void NotifyActivityActivated()
        {

        }

        public virtual void Inactivate()
        {
            this.IsActive = false;
        }

        public virtual void Remove(IContext context)
        {
            //if (this.Parent != null)
            //{
            //    this.Parent.children.Remove(this);
            //    var store = context.GetService<IInstanceStore>();
            //    store.Remove(this);
            //}
            //else
            //    throw new Exception("Can't delete root-token.");
        }

        public virtual Execution Root
        {
            get => this.ProcessInstance.Execution;
        }

        protected void CollectInactiveTokensAt(FlowNode node, IList<Execution> executions)
        {
            if (!this.IsActive && node.Id.Equals(this.node.Id))
                executions.Add(this);

            var children = this.children;
            foreach (var child in children)
                child.CollectInactiveTokensAt(node, executions);
        }

        protected void CollectActiveTokens(IList<Execution> executions)
        {
            if (this.IsActive)
                executions.Add(this);

            var children = this.children;
            foreach (var child in children)
                child.CollectActiveTokens(executions);
        }

        public virtual IList<Execution> GetInactiveExecutionsAt(FlowNode node)
        {
            var list = new List<Execution>();
            this.CollectInactiveTokensAt(node, list);

            return list;
        }

        public virtual IReadOnlyList<Execution> Children
        {
            get => this.children.ToList();
        }

        public virtual IReadOnlyList<Variable> Variables
        {
            get => this.variables.ToList();
        }

        protected virtual void OnNodeChanged()
        {
            this.ActivityId = this.node?.Id;
        }

        protected virtual void EnsureInitialized()
        {
            if (!this.isInitialized)
            {
                //if (this.node == null && this.ActivityId != null)
                //{
                //    var procDef = this.ProcessInstance.ProcessDefinition;
                //    var deploymentId = procDef.DeploymentId;
                //    var dm = Context.Current.GetService<IDeploymentManager>();
                //    var model = dm.GetBpmnModel(deploymentId);
                //    this.node = model?.GetFlowElement(this.ActivityId) as FlowNode;
                //}

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
            //set
            //{
            //    this.node = value;
            //    this.ActivityId = value?.Id;
            //}
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

        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual ProcessInstance CreateSubProcessInstance(IContext context)
        {
            var procInst = new ProcessInstance(null, null);

            return procInst;
        }

        public virtual Execution CreateExecution(IContext context)
        {
            var execution = new Execution(this);
            this.children.Add(execution);

            //var store = context.GetService<IInstanceStore>();
            //store.Add(token);

            return execution;
        }

        protected IContext context;

        public virtual void Start(IContext context, FlowNode initialNode)
        {
            this.context = context;

            this.EnterNode(initialNode);
        }

        protected virtual void EnterNode(FlowNode node)
        {
            this.node = node;

            var behavior = this.node.Tag as IFlowNodeBehavior;
            if (behavior != null)
                throw new NotSupportedException("The flowNode not supported.");

            var result = behavior.CanActivate(this);
            if (result)
            {
                this.NotifyActivityActivated();

                this.Transition = null;

                behavior.Execute(this);
            }
        }

        internal virtual Execution Join(IList<Execution> executions)
        {
            return null;
        }

        internal virtual void LeaveNode(SequenceFlow transition)
        {
            this.Transition = transition;

            var target = this.Transition.TargetRef;
            this.EnterNode(target);
        }

        public virtual SequenceFlow Transition
        {
            get;
            protected set;
        }

        internal virtual void LeaveNode(IList<SequenceFlow> outgoings, IList<Execution> joinedExecutions)
        {
            var scopeExecution = this.ResolveScope();
            var parents = new List<Execution>();
            var items = new List<Execution>(joinedExecutions);
            foreach(var item in joinedExecutions)
            {
                item.Remove(this.context);

                if(item.Parent != null && !parents.Contains(item.Parent))
                    parents.Add(item.Parent);
            }

            foreach(var item in parents)
            {
                if(item.children.Count == 0)
                {
                    item.Remove(this.context);
                }
            }

            var currentExecution = this;
            while(currentExecution.Parent != scopeExecution)
            {
                if (currentExecution.Parent.children.Count > 1)
                    break;

                currentExecution.Remove(context);
                currentExecution = currentExecution.Parent;
            }

            if(this.Equals(currentExecution))
            {
                //replace execution.
                currentExecution.node = this.node;
                currentExecution.ActivityId = this.ActivityId;
                currentExecution.context = this.context;
            }

            var concurrentExecutions = new List<Execution>();

            foreach (var outgoing in outgoings)
            {
                var item = new Execution(currentExecution);
                concurrentExecutions.Add(item);
            }

            //foreach (var outgoing in concurrentExecutions)
            //{ 

            //}
        }

        protected virtual void OnLeave(IContext context)
        {
            //fire leave event.
        }

        //protected virtual void Take(IContext context)
        //{
        //    if (this.Transition == null)
        //        throw new InvalidOperationException("The outgoing transition was not specified.");

        //    this.node = null;
        //    var target = this.Transition.TargetRef;

        //    //target.Enter(this);
        //}

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

        public virtual object GetVariable(string name)
        {
            if (this.variableByName == null)
                this.variableByName = this.variables.ToDictionary(x => x.Name);

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
                return variable.GetValue();

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

            ////Try update historic data.
            //if (this.ActivityInstance != null)
            //    this.ActivityInstance.SetVariable(name, value);
        }

        public virtual Execution ResolveScope()
        {
            if (this.Scope == null)
                return this.ProcessInstance.Execution;

            var activityInstanceId = this.Scope.Id;
            var p = this.Parent;
            while (p != null && p.ActivityInstance != null)
            {
                if (activityInstanceId == p.ActivityInstance.Id)
                    return p;

                p = p.Parent;
            }

            return this.ProcessInstance.Execution;
        }

        public virtual IList<Execution> GetActiveExecutions()
        {
            var list = new List<Execution>();
            this.CollectActiveTokens(list);

            return list;
        }

        public virtual void Take(SequenceFlow transition)
        {

        }

        public virtual void TakeAll()
        {

        }

        internal void NotifyActivityCompleted()
        {

        }

        public virtual void End(bool isImplicit = false, string endReason = null)
        {
            if (this.children.Count > 0)
                return;

            if(this.node is SubProcess)
            {
                var behavior = (this.node as SubProcess).Tag as IFlowNodeBehavior;
                behavior.Leave(this);
                return;
            }

            this.Inactivate();
            this.NotifyActivityCompleted();

            if(this.Parent != null)
            {
                this.Remove(context);

                this.Parent.End();
                return;
                //var parentToken = this.Parent;

                ////判断是否在子流程中
                //var container = this.node.Container;
                //if (container is SubProcess)
                //{
                //    this.Remove(context);

                //    if (parentToken.children.Count > 0)
                //        return;

                //    var subProcess = container as SubProcess;

                //    //删除并发Token
                //    var p = parentToken;
                //    while (!p.Node.Equals(subProcess))
                //    {
                //        if (p.children.Count > 0) //还有未完成的并发执行
                //            return;

                //        p.Remove(context);
                //        p = p.Parent;
                //    }

 

                //    //subProcess.Leave(ExecutionContext.Create(context, p));
                //    return;
                //}

            }

            //结束流程实例
            this.ProcessInstance.End(context);
        }

        public virtual void End(IContext context,
            bool isImplicit = false, string endReason = null)
        {
            //complete act-inst.
            if (this.ActivityInstance != null)
                this.ActivityInstance.Finish();

            this.Inactivate();

            //var store = context.GetService<IInstanceStore>();
            //store.Add(new HistoricToken(ExecutionContext.Create(context, this), "end"));

            //if (this.Parent != null)
            //{
            //    var parentToken = this.Parent;

            //    //判断是否在子流程中
            //    var container = this.node.Container;
            //    if (container is SubProcess)
            //    {
            //        this.Remove(context);

            //        if (parentToken.children.Count > 0)
            //            return;

            //        var subProcess = container as SubProcess;

            //        //删除并发Token
            //        var p = parentToken;
            //        while (!p.Node.Equals(subProcess))
            //        {
            //            if (p.children.Count > 0) //还有未完成的并发执行
            //                return;

            //            p.Remove(context);
            //            p = p.Parent;
            //        }

            //        subProcess.Leave(ExecutionContext.Create(context, p));
            //        return;
            //    }
            //}

            ////结束流程实例
            //this.ProcessInstance.End(context, isImplicit, endReason);
        }

        /// <summary>
        /// Sends a signal to this token. leaves the current {@link #getNode() node} over the default transition
        /// </summary>
        public virtual void Signal(IContext context,
            string signalEvent = null,
            IDictionary<string, object> signalData = null)
        {
            if (this.node == null)
                throw new RuntimeException(this + " is not positioned in a node");

            this.context = context;

            var behavior = this.node.Tag as ISignallable;
            if (behavior == null)
                throw new NotSupportedException("The flowNode does not support signal event.");

            behavior.Signal(this, signalEvent, signalData);
        }

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

        public virtual void Terminate(IContext context,
            string endReason = null)
        {
            var tokens = this.children.ToList();

            foreach (var child in tokens)
            {
                child.Terminate(context, endReason);
                this.children.Remove(child);
            }

            this.Inactivate();

            if (this.ActivityInstance != null)
                this.ActivityInstance.Terminate(context, endReason);
        }

        public virtual void Resume(IContext context)
        {
            this.IsSuspended = false;
        }

        public virtual void Suspend(IContext context)
        {
            this.IsSuspended = false;
        }
    }
}
