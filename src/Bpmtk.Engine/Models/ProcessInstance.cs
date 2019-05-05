using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Variables;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine.Models
{
    public class ProcessInstance : ExecutionObject, IProcessInstance
    {
        private IDictionary<string, Variable> variableByName;

        public virtual string TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Business object unique-key.
        /// </summary>
        public virtual string Key
        {
            get;
            set;
        }
        
        public virtual User Initiator
        {
            get;
            set;
        }

        public virtual string EndReason
        {
            get;
            set;
        }

        public override IReadOnlyList<IVariable> VariableInstances => this.Variables.ToList();

        public virtual ICollection<Variable> Variables
        {
            get;
            set;
        }

        public virtual long? SuperId
        {
            get;
            set;
        }

        public virtual Token Super
        {
            get;
            set;
        }

        public virtual ICollection<Token> Tokens
        {
            get;
            set;
        }

        //public ProcessInstance()
        //{
        //    this.IdentityLinks = new List<IdentityLink>();
        //    this.Variables = new List<Variable>();
        //}

        //public virtual bool GetVariable(string name, out object value)
        //{
        //    this.EnsureVariablesInitialized();

        //    value = null;
        //    Variable variable = null;

        //    if (this.variableByName.TryGetValue(name, out variable))
        //    {
        //        value = variable.GetValue();
        //        return true;
        //    }

        //    return false;
        //}

        protected virtual void EnsureVariablesInitialized()
        {
            if (this.variableByName != null)
                return;

            if(this.Variables != null)
                this.variableByName = this.Variables.ToDictionary(x => x.Name);
        }

        public override void SetVariable(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.EnsureVariablesInitialized();

            Variable variable = null;
            if (this.variableByName.TryGetValue(name, out variable))
            { 
                variable.SetValue(value);
                return;
            }

            this.AddVariable(name,  value);
        }

        public virtual bool HasVariable(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.EnsureVariablesInitialized();

            return this.variableByName.ContainsKey(name);
        }

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

        public override object GetVariable(string name)
            => this.FindVariableByName(name)?.GetValue();

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

        public virtual ProcessDefinition ProcessDefinition
        {
            get;
            set;
        }

        public virtual ActivityInstance Caller
        {
            get;
            set;
        }

        public virtual IList<Token> GetInactiveTokensAt(string activityId)
        {
            var rootTokens = this.Tokens.Where(x => x.Parent == null)
                    .ToList();

            var list = new List<Token>();

            foreach(var token in rootTokens)
            {
                var items = token.GetInactiveTokensAt(activityId);
                if(items.Count > 0)
                    list.AddRange(items);
            }

            return list;
        }

        IUser IProcessInstance.Initiator => this.Initiator;

        //public virtual void Start(IContext context,
        //    FlowNode initialNode)
        //{
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));

        //    if (initialNode == null)
        //        throw new ArgumentNullException(nameof(initialNode));

        //    if (this.token != null)
        //        throw new RuntimeException("The process instance has already started.");

        //    var store = context.GetService<IInstanceStore>();

        //    var rootToken = new Token(this, initialNode);          
        //    this.token = rootToken;

        //    //store.Add(rootToken);

        //    this.StartTime = DateTime.Now;
        //    this.State = ExecutionState.Active;
            
        //    store.UpdateAsync(this).GetAwaiter().GetResult();

        //    var executionContext = ExecutionContext.Create(context, this.token);
        //    executionContext.Start(initialNode);

        //    //fire ProcessStartEvent
        //    //this.OnStart(context, initialNode);
        //    //this.token.Start(context, initialNode);
        //}

        //protected virtual void OnStart(IContext context, FlowNode initialNode)
        //{
        //    // fire the process start event
        //    if (initialNode != null)
        //    {
        //        var executionContext = ExecutionContext.Create(context, this.execution);

        //        //execute the start node
        //        initialNode.Enter(executionContext);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isImplicit">indicates end from endEvent</param>
        /// <param name="endReason"></param>
        //public virtual void End(IContext context, bool isImplicit = false, 
        //    string endReason = null)
        //{
        //    var rootToken = this.token;
        //    if(isImplicit)
        //    {
        //        var activeTokens = rootToken.GetActiveTokens();
        //        if (activeTokens.Count > 0)
        //            return;
        //    }

        //    //Clear
        //    this.token = null;

        //    //Remove root-token.
        //    var store = Context.Current.GetService<IInstanceStore>();
        //    store.UpdateAsync(this).GetAwaiter().GetResult();
        //    store.Remove(rootToken);

        //    this.State = ExecutionState.Completed;
        //    this.LastStateTime = Clock.Now;

        //    //判断CallActivity
        //    if (this.super != null)
        //    {
        //        var executionContext = ExecutionContext.Create(context, this.super);
        //        executionContext.SubProcessInstance = this;

        //        executionContext.LeaveNode();
        //    }
        //}

        /// <summary>
        /// Terminate the process-instance.
        /// </summary>
        //public override void Terminate(IContext context, string endReason)
        //{
        //    if (this.token == null)
        //        throw new EngineException("The process-instance has ended already.");

        //    this.State = ExecutionState.Terminated;
        //    this.EndReason = endReason;
        //    this.LastStateTime = Clock.Now;

        //    this.token.Terminate(context, endReason);

        //    //terminate all active tasks
        //    var taskStore = context.GetService<ITaskStore>();
        //    var tasks = taskStore.CreateQuery()
        //        .SetProcessInstanceId(this.Id)
        //        .List();

        //    foreach (var task in tasks)
        //        task.TerminateInternal(context);
        //}
    }
}
