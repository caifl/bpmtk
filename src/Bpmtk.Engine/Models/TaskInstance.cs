using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Models
{
    public class TaskInstance : ITaskInstance
    {
        protected ProcessInstance processInstance;
        protected ActivityInstance activityInstance;
        protected bool isSuspended;
        protected IDictionary<string, Variable> variableByName;
        //protected ICollection<IdentityLink> identityLinks;

        //protected TaskInstance()
        //{ }

        //public TaskInstance(Token token)
        //{
        //    this.identityLinks = new List<IdentityLink>();

        //    this.Token = token;
        //    this.processInstance = token.ProcessInstance;
        //    this.activityInstance = token.ActivityInstance;
        //    this.Created = Clock.Now;
        //    this.State = TaskState.Active;
        //    this.ActivityId = token.ActivityId;
        //    this.LastStateTime = this.Created;

        //    this.ProcessInstanceId = this.processInstance?.Id;
        //}
        public virtual ICollection<Variable> Variables
        {
            get;
            set;
        }

        public virtual long Id
        {
            get;
            set;
        }

        //public virtual ProcessDefinition ProcessDefinition
        //{
        //    get;
        //    set;
        //}

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual long? ProcessInstanceId
        {
            get;
            set;
        }

        public virtual long? ActivityInstanceId
        {
            get;
            set;
        }

        public virtual ActivityInstance ActivityInstance
        {
            get;
            set;
        }

        public virtual TaskState State
        {
            get;
            set;
        }

        public virtual DateTime LastStateTime
        {
            get;
            set;
        }

        public virtual long? TokenId
        {
            get;
            set;
        }

        public virtual Token Token
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual short Priority
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual DateTime? ClaimedTime
        {
            get;
            set;
        }

        public virtual string Assignee
        {
            get;
            set;
        }

        public virtual DateTime? DueDate
        {
            get;
            set;
        }

        public virtual DateTime Modified
        {
            get;
            set;
        }

        public virtual string ConcurrencyStamp
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual void AddIdentityLink(string userId, string type)
        {
            var item = new IdentityLink();
            item.UserId = userId;
            item.Type = type;

            //this.identityLinks.Add(item);
        }

        public virtual ICollection<IdentityLink> IdentityLinks
        {
            get;
            set;
        }

        //public virtual void Resume(IContext context)
        //{
        //    this.isSuspended = false;
        //    this.Token.Resume(context);
        //}

        //public virtual void Suspend(IContext context)
        //{
        //    this.isSuspended = true;
        //    this.Token.Suspend(context);
        //}

        public virtual bool IsClosed
        {
            get => this.State == TaskState.Completed || this.State == TaskState.Terminated;
        }

        //internal virtual void TerminateInternal(IContext context)
        //{
        //    this.State = TaskState.Terminated;
        //    this.LastStateTime = Clock.Now;
        //}

        //public virtual void Terminate(IContext context, string endReason)
        //{
        //    if (this.IsClosed)
        //        throw new EngineException("The task has closed already.");

        //    if (this.processInstance != null)
        //        this.processInstance.Terminate(context, endReason);
        //}
        protected virtual void EnsureVariablesInitialized()
        {
            if (this.variableByName != null)
                return;

            if(this.Variables != null)
                this.variableByName = this.Variables.ToDictionary(x => x.Name);
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

        public virtual object GetVariableLocal(string name)
            => this.FindVariableByName(name)?.GetValue();

        public virtual object GetVariable(string name)
        {
            Variable variable = this.FindVariableByName(name);
            if (variable != null)
                return variable.GetValue();

            if (this.Token != null)
                return this.Token.GetVariable(name);
            else if (this.ActivityInstance != null)
                return this.ActivityInstance.GetVariable(name);
            else if (this.ProcessInstance != null)
                return this.ProcessInstance.GetVariable(name);

            return null;
        }

        public virtual void SetVariable(string name, object value)
        {
            Variable variable = this.FindVariableByName(name);
            if (variable != null)
            {
                variable.SetValue(value);
                return;
            }

            if (this.Token != null)
                this.Token.SetVariable(name, value);
            else if (this.ActivityInstance != null)
                this.ActivityInstance.SetVariable(name, value);
            else if (this.ProcessInstance != null)
                this.ProcessInstance.SetVariable(name, value);
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

        IDictionary<string, object> ITaskInstance.ProcessVariables
        {
            get
            {
                var map = new Dictionary<string, object>();

                if (this.ProcessInstance != null)
                {
                    var items = this.ProcessInstance.Variables;
                    foreach (var item in items)
                        map.Add(item.Name, item.GetValue());
                }

                return map;
            }
        }
    }
}
