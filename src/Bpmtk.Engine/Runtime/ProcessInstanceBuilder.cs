using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    class ProcessInstanceBuilder : IProcessInstanceBuilder
    {
        protected Token super;
        protected ProcessDefinition processDefinition;
        protected IDictionary<string, object> variables;
        private readonly RuntimeManager runtimeManager;
        private readonly Context context;

        public ProcessInstanceBuilder(RuntimeManager runtimeManager)
        {
            this.runtimeManager = runtimeManager;
            this.context = runtimeManager.Context;
        }

        public virtual async Task<ProcessInstance> BuildAsync()
        {
            if (this.processDefinition == null)
                throw new ArgumentNullException(nameof(processDefinition));

            var pi = new ProcessInstance();
            pi.ProcessDefinition = this.processDefinition;
            pi.Super = this.super;
            //pi.variableInstances = new List<ProcessVariable>();
            //pi.variables = new Dictionary<string, ProcessVariable>();
            //pi.identityLinks = new List<InstanceIdentityLink>();

            if (pi.Super != null)
                pi.Caller = super.ActivityInstance;

            pi.State = ExecutionState.Ready;

            pi.Name = processDefinition.Name;
            if (string.IsNullOrEmpty(pi.Name))
                pi.Name = processDefinition.Key;

            pi.Created = Clock.Now;
            pi.LastStateTime = pi.Created;
            pi.Initiator = null;
            pi.Description = processDefinition.Description;

            //initialize context.
            await this.InitializeContextAsync(pi);

            //save.
            await this.context.DbSession.SaveAsync(pi);

            return pi;
        }

        protected virtual async Task InitializeContextAsync(ProcessInstance processInstance)
        {
            var deploymentId = this.processDefinition.Deployment.Id;
            var processDefinitionKey = this.processDefinition.Key;

            var dm = context.DeploymentManager;
            var model = await dm.GetBpmnModelAsync(deploymentId);
            var dataObjects = model.GetProcessDataObjects(processDefinitionKey);

            var dataObjectsMap = dataObjects.ToDictionary(x => x.Id);

            IVariableType type = null;
            foreach (var dataObject in dataObjects)
            {
                var value = dataObject.Value;
                type = Variables.VariableType.Resolve(value);

                //this.CreateVariableInstance(dataObject.Id, type, value);
            }

            if (variables != null && variables.Count > 0)
            {
                var em = variables.GetEnumerator();
                //while (em.MoveNext())
                //{
                //    this.SetVariable(em.Current.Key, em.Current.Value);
                //}
            }
        }

        public virtual IProcessInstanceBuilder SetProcessDefinition(ProcessDefinition processDefinition)
        {
            this.processDefinition = processDefinition;

            return this;
        }
    }
}
