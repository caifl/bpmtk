using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Query;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Events;

namespace Bpmtk.Engine.Internal
{
    public class ExecutionService : IRuntimeService
    {
        private readonly IInstanceStore executions;
        protected readonly IDeploymentStore deployments;
        private readonly IEventSubscriptionStore eventSubscriptions;

        public ExecutionService(IInstanceStore executions,
            IDeploymentStore deployments,
            IEventSubscriptionStore eventSubscriptions)
        {
            this.executions = executions;
            this.deployments = deployments;
            this.eventSubscriptions = eventSubscriptions;
        }

        public virtual IProcessInstanceQuery CreateProcessInstanceQuery()
        {
            return null;
        }

        public virtual IEnumerable<string> GetActiveActivityIds(long id)
        {
            return this.executions.GetActiveActivityIds(id);
        }

        public virtual ProcessInstance StartProcessByKey(string processDefinitionKey,
            IDictionary<string, object> variables)
        {
            var processDefinition = deployments.GetProcessDefintionByKey(processDefinitionKey);
            if (processDefinition == null)
                throw new KeyNotFoundException("The specified process-definition was not found.");

            var pi = new ProcessInstance(processDefinition);
            this.executions.Add(pi);

            var context = Context.Current;
            pi.Start(context);

            return pi;
        }

        IProcessInstance IRuntimeService.StartProcessInstanceByKey(string processDefinitionKey, IDictionary<string, object> variables)
            => this.StartProcessByKey(processDefinitionKey, variables);

        public virtual IProcessInstance FindProcessInstanceById(long id)
        {
            return this.executions.Find(id);
        }

        public virtual Task<IEnumerable<IdentityLink>> GetIdentityLinksAsync(long id)
        {
            return null;
        }

        public virtual Task<IdentityLink> AddIdentityLink(long id, User user, 
            string linkType = null)
        {
            return null;
        }

        public virtual Task<IdentityLink> AddIdentityLink(long id, Group group,
            string linkType = null)
        {
            return null;
        }

        public virtual Task RemoveIdentityLinksAsync(long id, params long[] identityLinks)
        {
            return null;
        }

        public virtual Task SetProcessInstanceKeyAsync(long id, string key)
        {
            var inst = this.executions.Find(id);
            inst.Key = key;

            return this.executions.UpdateAsync(inst);
        }

        public virtual Task SetProcessInstanceNameAsync(long id, string name)
        {
            var inst = this.executions.Find(id);
            inst.Name = name;

            return this.executions.UpdateAsync(inst);
        }

        public virtual IProcessInstance StartProcessInstanceByMessage(string messageName, object messageData = null)
        {
            var eventSubscr = this.eventSubscriptions.FindByName(messageName, "message");
            if (eventSubscr == null)
                throw new Bpmn2.BpmnError($"The message '{messageName}' event handler does not exists.");

            if(eventSubscr.ProcessDefinition != null
                && eventSubscr.ActivityId != null)
            {
                IMessageStartEventHandler handler = null;
                var task = handler.Execute(eventSubscr, messageData);

                return task.Result;
            }

            return null;
        }
    }
}
