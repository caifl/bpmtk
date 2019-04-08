using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Query;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Events.Internal;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.Internal
{
    public class ExecutionService : IRuntimeService
    {
        private readonly IInstanceStore instances;
        protected readonly IDeploymentManager deploymentManager;
        private readonly IEventSubscriptionStore eventSubscriptions;

        public ExecutionService(IInstanceStore instances,
            IDeploymentManager deploymentManager,
            IEventSubscriptionStore eventSubscriptions)
        {
            this.instances = instances;
            this.deploymentManager = deploymentManager;
            this.eventSubscriptions = eventSubscriptions;
        }

        public virtual IProcessInstanceQuery CreateProcessQuery()
        {
            return null;
        }

        public virtual IEnumerable<string> GetActiveActivityIds(long id)
        {
            return this.instances.GetActiveActivityIds(id);
        }

        public virtual ProcessInstance StartProcessByKey(string processDefinitionKey,
            IDictionary<string, object> variables)
        {
            var processDefinition = this.deploymentManager.FindLatestProcessDefinitionByKey(processDefinitionKey);
            if (processDefinition == null)
                throw new KeyNotFoundException("The specified process-definition was not found.");

            var model = this.deploymentManager.GetBpmnModel(processDefinition.DeploymentId);
            var process = model.GetProcess(processDefinition.Key);
            if (process == null)
                throw new RuntimeException($"The BPMN model not contains process '{processDefinition.Key}'");

            var initialNode = process.InitialNode;
            if (initialNode == null)
                throw new RuntimeException($"No start element found for process definition " + processDefinition.Key) ;

            var pi = new ProcessInstance(processDefinition);
            
            var context = Context.Current;

            pi.Initiator = new User() { Id = context.UserId };

            pi.InitializeContext(context, variables);

            this.instances.Add(pi);
            pi.Start(context, initialNode);

            return pi;
        }

        IProcessInstance IRuntimeService.StartProcessInstanceByKey(string processDefinitionKey, IDictionary<string, object> variables)
            => this.StartProcessByKey(processDefinitionKey, variables);

        public virtual IProcessInstance FindProcessInstanceById(long id)
        {
            return this.instances.Find(id);
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
            var inst = this.instances.Find(id);
            inst.Key = key;

            return this.instances.UpdateAsync(inst);
        }

        public virtual Task SetProcessInstanceNameAsync(long id, string name)
        {
            var inst = this.instances.Find(id);
            inst.Name = name;

            return this.instances.UpdateAsync(inst);
        }

        public virtual IProcessInstance StartProcessInstanceByMessage(string messageName, object messageData = null)
        {
            var eventSubscr = this.eventSubscriptions.FindByName(messageName, "message");
            if (eventSubscr == null)
                throw new RuntimeException($"The message '{messageName}' event handler does not exists.");

            if(eventSubscr.ProcessDefinition != null
                && eventSubscr.ActivityId != null)
            {
                IMessageStartEventHandler handler = new MessageStartEventHandler(this.deploymentManager,
                    this.instances);

                var task = handler.Execute(eventSubscr, messageData);

                return task.Result;
            }

            return null;
        }

        public IActivityInstanceQuery CreateActivityQuery()
            => this.instances.CreateActivityQuery();

        public virtual ITokenQuery CreateTokenQuery()
            => this.instances.CreateTokenQuery();

        public virtual void Trigger(long tokenId)
        {
            var token = this.instances.FindToken(tokenId);
            token.Signal(Context.Current);
        }
    }
}
