using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    class ProcessInstanceBuilder : IProcessInstanceBuilder
    {
        protected string key;
        protected string name;
        protected string description;
        protected int? initiatorId;
        protected Token super;
        protected ProcessDefinition processDefinition;
        protected IDictionary<string, object> variables;
        private readonly Context context;

        protected List<Bpmtk.Bpmn2.FlowNode> initialNodes = new List<Bpmtk.Bpmn2.FlowNode>();

        public virtual IList<Bpmtk.Bpmn2.FlowNode> InitialNodes => this.initialNodes;

        public ProcessInstanceBuilder(RuntimeManager runtimeManager)
        {
            this.context = runtimeManager.Context;
        }

        public virtual async Task<ProcessInstance> BuildAsync()
        {
            if (this.processDefinition == null)
                throw new ArgumentNullException(nameof(processDefinition));

            //Load bpmn-model.
            var deploymentId = this.processDefinition.DeploymentId;
            var processDefinitionKey = this.processDefinition.Key;

            var dm = context.DeploymentManager;
            var model = await dm.GetBpmnModelAsync(deploymentId);
            var process = model.GetProcess(processDefinitionKey);
            var initialNode = process.InitialNode;
            if (initialNode == null)
                throw new RuntimeException($"The process '{processDefinitionKey}' does not contains any start nodes.");

            this.initialNodes.Clear();
            this.initialNodes.Add(initialNode);

            //init process-instance.
            var pi = new ProcessInstance();
            pi.Tokens = new List<Token>();
            
            pi.ProcessDefinition = this.processDefinition;
            pi.Super = this.super;

            if (pi.Super != null)
                pi.Caller = super.ActivityInstance;

            pi.State = ExecutionState.Ready;

            pi.Name = processDefinition.Name;
            if (string.IsNullOrEmpty(pi.Name))
                pi.Name = processDefinition.Key;

            pi.Key = this.key;
            pi.Created = Clock.Now;
            pi.LastStateTime = pi.Created;

            if(this.initiatorId != null)
                pi.Initiator = await this.context.IdentityManager
                    .FindUserByIdAsync(this.initiatorId.Value);

            pi.Description = processDefinition.Description;

            if (!string.IsNullOrEmpty(this.name))
                pi.Name = this.name;

            if (!string.IsNullOrEmpty(this.description))
                pi.Description = this.description;

            pi.Variables = new List<Variable>();
            pi.IdentityLinks = new List<IdentityLink>();

            var dataObjects = model.GetProcessDataObjects(processDefinitionKey);

            //initialize context.
            this.InitializeProcessContext(pi, dataObjects);

            //save.
            await this.context.DbSession.SaveAsync(pi);

            //commit changes.
            await this.context.DbSession.FlushAsync();

            return pi;
        }

        protected virtual void InitializeProcessContext(ProcessInstance processInstance,
            IList<ValuedDataObject> dataObjects)
        {
            var dataObjectsMap = dataObjects.ToDictionary(x => x.Id);

            IVariableType type = null;
            foreach (var dataObject in dataObjects)
            {
                var value = dataObject.Value;
                type = Variables.VariableType.Resolve(value);

                var variable = new Variable();
                variable.Name = dataObject.Id;
                variable.Type = type.Name;

                type.SetValue(variable, value);

                processInstance.Variables.Add(variable);
            }

            if (variables != null && variables.Count > 0)
            {
                var em = variables.GetEnumerator();
                while (em.MoveNext())
                {
                    processInstance.SetVariable(em.Current.Key, em.Current.Value);
                }
            }
        }

        public virtual IProcessInstanceBuilder SetProcessDefinition(ProcessDefinition processDefinition)
        {
            this.processDefinition = processDefinition;

            return this;
        }

        public IProcessInstanceBuilder SetVariables(IDictionary<string, object> variables)
        {
            this.variables = variables;

            return this;
        }

        public virtual IProcessInstanceBuilder SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual IProcessInstanceBuilder SetKey(string key)
        {
            this.key = key;

            return this;
        }

        public virtual IProcessInstanceBuilder SetInitiator(int initiatorId)
        {
            this.initiatorId = initiatorId;

            return this;
        }

        public virtual IProcessInstanceBuilder SetDescription(string description)
        {
            this.description = description;

            return this;
        }

        public virtual IProcessInstanceBuilder SetSuper(Token super)
        {
            this.super = super;

            return this;
        }
    }
}
