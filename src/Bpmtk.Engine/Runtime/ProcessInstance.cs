using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Variables;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Runtime
{
    public class ProcessInstance : ExecutionObject, IProcessInstance, IAggregateRoot
    {
        private Token super;
        private ActivityInstance callActivityInstance;

        private ProcessDefinition processDefinition;
        private ICollection<ProcessVariable> variableInstances;
        private ICollection<InstanceIdentityLink> identityLinks;
        private Token token;
        private IDictionary<string, ProcessVariable> variables;

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
            protected set;
        }

        public override IEnumerable<VariableInstance> VariableInstances => this.variableInstances;

        public virtual IEnumerable<IdentityLink> IdentityLinks => this.identityLinks;

        public virtual Token Super
        {
            get => this.super;
            protected set => this.super = value;
        }

        public virtual IReadOnlyCollection<Token> Tokens
        {
            get;
        }

        public virtual Token Token
        {
            get => this.token;
            protected set => this.token = value;
        }

        protected ProcessInstance()
        { }

        public ProcessInstance(ProcessDefinition processDefinition, 
            Token super = null)
        {
            if(processDefinition == null)
                throw new ArgumentNullException(nameof(processDefinition));

            this.super = super;
            this.variableInstances = new List<ProcessVariable>();
            this.variables = new Dictionary<string, ProcessVariable>();
            this.identityLinks = new List<InstanceIdentityLink>();
            this.processDefinition = processDefinition;

            if (this.super != null)
                this.callActivityInstance = super.ActivityInstance;

            this.State = ExecutionState.Ready;

            this.Name = processDefinition.Name;
            if (string.IsNullOrEmpty(this.Name))
                this.Name = processDefinition.Key;

            this.Created = Clock.Now;
            this.Initiator = null;
            this.Description = processDefinition.Description;
        }

        IDictionary<string, ValuedDataObject> dataObjects = null;

        public virtual void InitializeContext(IContext context)
        {
            var deploymentId = this.processDefinition.Deployment.Id;
            var processDefinitionKey = this.processDefinition.Key;

            var dm = context.GetService<IDeploymentManager>();
            var model = dm.GetBpmnModel(deploymentId);
            var dataObjects = model.GetProcessDataObjects(processDefinitionKey);

            this.dataObjects = dataObjects.ToDictionary(x => x.Id);

            IVariableType type = null;
            foreach (var dataObject in dataObjects)
            {
                var value = dataObject.Value;
                type = Variables.VariableType.Resolve(value);

                this.CreateVariableInstance(dataObject.Id, type, value);
            }
        }

        public virtual bool GetVariable(string name, out object value)
        {
            value = null;
            ProcessVariable variable = null;

            if (this.variables.TryGetValue(name, out variable))
            {
                value = variable.GetValue();
                return true;
            }

            return false;
        }

        public override void SetVariable(string name, object value)
        {
            ProcessVariable variable = null;
            if (this.variables.TryGetValue(name, out variable))
            {
                variable.SetValue(value);
            }
            else
            {
                this.CreateVariableInstance(name, VariableType.Resolve(value), value);
            }
        }

        object IProcessInstance.GetVariable(string name)
            => this.GetVariable(name);

        public override object GetVariable(string name)
        {
            object value = null;
            if (this.GetVariable(name, out value))
                return value;

            return null;
        }

        protected virtual ProcessVariable CreateVariableInstance(string name, 
            IVariableType type, 
            object initialValue = null)
        {
            var item = new ProcessVariable(this, name, type, initialValue);
            this.variableInstances.Add(item);
            this.variables.Add(item.Name, item);

            return item;
        }

        public virtual VariableInstance AddVariable(string name, object value)
        {
            return null;
        }

        public virtual ProcessDefinition ProcessDefinition
        {
            get => this.processDefinition;
            protected set => this.processDefinition = value;
        }

        public virtual ActivityInstance CallActivityInstance
        {
            get => this.callActivityInstance;
            protected set => this.callActivityInstance = value;
        }

        public virtual void Start(IContext context,
            string initialActivityId = null)
        {
            if (this.token != null)
                throw new RuntimeException("The process instance has already started.");

            var dm = context.GetService<IDeploymentManager>();
            var model = dm.GetBpmnModel(this.processDefinition.DeploymentId);
            var process = model.GetProcess(this.processDefinition.Key);
            if (process == null)
                throw new BpmnError($"The BPMN model not contains process '{this.processDefinition.Key}'");

            FlowNode initialNode = null;
            if (initialActivityId != null)
            {
                initialNode = process.FlowElements.Where(x => x.Id == initialActivityId)
                    .OfType<StartEvent>()
                    .SingleOrDefault();
            }
            else
            {
                var startEvents = process.FlowElements
                    .OfType<StartEvent>();
                if (startEvents.Count() > 1)
                    throw new BpmnError($"The Process '{this.processDefinition.Key}' contains multiple startEvents.");

                initialNode = startEvents.FirstOrDefault();
            }

            var store = context.GetService<IInstanceStore>();

            var rootToken = new Token(this, initialNode);          
            this.token = rootToken;

            store.Add(rootToken);

            this.StartTime = DateTime.Now;
            this.State = ExecutionState.Active;
            
            store.UpdateAsync(this).GetAwaiter().GetResult();

            //fire ProcessStartEvent
            this.OnStart(context, initialNode);
        }

        protected virtual void OnStart(IContext context, FlowNode initialNode)
        {
            // fire the process start event
            if (initialNode != null)
            {
                var executionContext = ExecutionContext.Create(context, this.token);

                //CreateActivityInstance
                this.token.ActivityInstance = ActivityInstance.Create(executionContext);

                //processDefinition.fireEvent(Event.EVENTTYPE_PROCESS_START, executionContext);
                var store = context.GetService<IInstanceStore>();
                store.Add(new HistoricToken(executionContext, "start"));

                //execute the start node
                initialNode.Execute(executionContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isImplicit">indicates end from endEvent</param>
        /// <param name="endReason"></param>
        public virtual void End(IContext context, bool isImplicit = false, 
            string endReason = null)
        {
            var rootToken = this.token;
            if(isImplicit)
            {
                var activeTokens = rootToken.GetActiveTokens();
                if (activeTokens.Count > 0)
                    return;
            }

            //Clear
            this.token = null;

            //Remove root-token.
            var store = Context.Current.GetService<IInstanceStore>();
            store.UpdateAsync(this).GetAwaiter().GetResult();
            store.Remove(rootToken);

            this.State = ExecutionState.Completed;
            this.EndTime = Clock.Now;
                
            //判断CallActivity
            if(this.super != null)
            {
                var executionContext = ExecutionContext.Create(context, this.super);
                executionContext.SubProcessInstance = this;

                executionContext.LeaveNode();
            }
        }
    }
}
