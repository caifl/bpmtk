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
            this.identityLinks = new List<InstanceIdentityLink>();
            this.processDefinition = processDefinition;

            if (this.super != null)
                this.callActivityInstance = super.ActivityInstance;

            this.State = ExecutionState.Inactive;

            this.Name = processDefinition.Name;
            if (string.IsNullOrEmpty(this.Name))
                this.Name = processDefinition.Key;

            this.Created = Clock.Now;
            this.Initiator = null;
            this.Description = processDefinition.Description;

            this.InitializeContext();
        }

        protected virtual void InitializeContext()
        {
            //var deploymentId = this.processDefinition.Deployment.Id;
            //var processDefinitionKey = this.processDefinition.Key;

            //var dm = Context.GetService<IDeploymentManager>();

            //var model = dm.GetBpmnModel(deploymentId);
            //var process = model.GetProcess(processDefinitionKey);

            //var signatures = process.GetContextSignatures();

            //var list = new List<ProcessVariable>();
            //IDictionary<string, ProcessVariable> map = new Dictionary<string, ProcessVariable>();
            //var em = signatures.GetEnumerator();
            //string name = null;
            //IVariableType type = null;

            //while (em.MoveNext())
            //{
            //    name = em.Current.Key;
            //    type = em.Current.Value;

            //    var item = this.CreateVariableInstance(name, type);
            //    list.Add(item);
            //    map.Add(name, item);
            //}

            //this.variableInstances = list;
            //this.variables = map;
        }

        protected virtual ProcessVariable CreateVariableInstance(string name, 
            IVariableType type, 
            object initialValue = null)
        {
            var item = new ProcessVariable(this, name, initialValue);
            this.variableInstances.Add(item);

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

            var store = context.GetService<IProcessInstanceStore>();

            var rootToken = new Token(this, initialNode);          
            this.token = rootToken;

            store.Add(rootToken);

            this.StartTime = DateTime.Now;
            this.State = ExecutionState.Active;
            
            store.UpdateAsync(this).GetAwaiter().GetResult();

            //fire ProcessStartEvent
            this.OnStart(initialNode);
        }

        protected virtual void OnStart(FlowNode initialNode)
        {
            // fire the process start event
            if (initialNode != null)
            {
                var executionContext = new ExecutionContext(this.token);
                //processDefinition.fireEvent(Event.EVENTTYPE_PROCESS_START, executionContext);
                var store = executionContext.Context.GetService<IProcessInstanceStore>();
                store.Add(new HistoricToken(executionContext, "start"));

                //execute the start node
                initialNode.Execute(executionContext);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="isImplicit">indicates end from endEvent</param>
        ///// <param name="endReason"></param>
        public virtual void End(bool isImplicit = false, 
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
            var store = Context.Current.GetService<IProcessInstanceStore>();
            store.Remove(rootToken);

            this.State = ExecutionState.Completed;
            this.EndTime = Clock.Now;
                
            //判断CallActivity
            if(this.super != null)
            {
                var executionContext = new ExecutionContext(this.super);
                executionContext.SubProcessInstance = this;

                executionContext.LeaveNode();
            }
        }
    }
}
