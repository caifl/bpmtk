using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Models
{
    public class ProcessInstance : ExecutionObject
    {
        //private ICollection<Variable> variableInstances;
        private IDictionary<string, Variable> variables;

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

        public virtual bool GetVariable(string name, out object value)
        {
            value = null;
            Variable variable = null;

            if (this.variables.TryGetValue(name, out variable))
            {
                value = variable.GetValue();
                return true;
            }

            return false;
        }

        public override void SetVariable(string name, object value)
        {
            Variable variable = null;
            if (this.variables.TryGetValue(name, out variable))
            {
                variable.SetValue(value);
            }
            else
            {
                this.CreateVariableInstance(name, VariableType.Resolve(value), value);
            }
        }

        public override Variable GetVariable(string name)
        {
            Variable value = null;
            if (this.variables.TryGetValue(name, out value))
                return value;

            return null;
        }

        protected virtual Variable CreateVariableInstance(string name, 
            IVariableType type, 
            object initialValue = null)
        {
            var item = new Variable();
            item.Name = name;
            //ProcessVariable(this, name, type, initialValue);
            //this.variableInstances.Add(item);
            this.variables.Add(item.Name, item);

            return item;
        }

        public virtual Variable AddVariable(string name, object value)
        {
            return null;
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
