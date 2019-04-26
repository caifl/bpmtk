using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Events;

namespace Bpmtk.Engine.Runtime
{
    public class RuntimeManager : IRuntimeManager
    {
        private readonly Context context;
        private readonly IDbSession db;
        protected readonly IDeploymentManager deploymentManager;

        public virtual IQueryable<ProcessInstance> ProcessInstances => this.db.ProcessInstances;

        public virtual IQueryable<Token> Tokens => this.db.Tokens;

        public RuntimeManager(Context context)
        {
            this.context = context;
            this.db = context.DbSession;
        }

        public virtual Context Context
        {
            get => this.context;
        }

        public virtual IProcessInstanceQuery CreateProcessQuery()
        {
            return null;
        }

        public virtual async Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId)
        {
            var query = this.db.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct();

            return await this.db.QueryMultipleAsync(query);
        }

        public virtual async Task<ProcessInstance> StartProcessByKeyAsync(string processDefinitionKey,
            IDictionary<string, object> variables)
        {
            var processDefinition = await this.deploymentManager.FindProcessDefinitionByKeyAsync(processDefinitionKey);
            if (processDefinition == null)
                throw new KeyNotFoundException("The specified process-definition was not found.");

            var model = await this.deploymentManager.GetBpmnModelAsync(processDefinition.DeploymentId);
            var process = model.GetProcess(processDefinition.Key);
            if (process == null)
                throw new RuntimeException($"The BPMN model not contains process '{processDefinition.Key}'");

            var initialNode = process.InitialNode;
            if (initialNode == null)
                throw new RuntimeException($"No start element found for process definition " + processDefinition.Key) ;

            var pi = new ProcessInstance();
            pi.ProcessDefinition = processDefinition;
            
            var context = Context.Current;

            pi.Initiator = new User() { Id = context.UserId };

            //pi.InitializeContext(context, variables);

            await this.db.SaveAsync(pi);

            //pi.Start(context, initialNode);

            return pi;
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

        public virtual async Task SetProcessInstanceKeyAsync(long processInstanceId, string key)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Key = key;

            await this.db.UpdateAsync(inst);
        }

        public virtual async Task SetProcessInstanceNameAsync(long processInstanceId, 
            string name)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Name = name;

            await this.db.UpdateAsync(inst);
        }

        public virtual async Task<ProcessInstance> StartProcessByMessageAsync(string messageName, 
            IDictionary<string, object> messageData = null)
        {
            var query = this.db.EventSubscriptions
                .Where(x => x.EventType == "message"
                    && x.EventName == messageName
                  );

            var eventSubscr = await this.db.QuerySingleAsync(query);
            if (eventSubscr == null)
                throw new RuntimeException($"The message '{messageName}' event handler does not exists.");

            if(eventSubscr.ProcessDefinition != null
                && eventSubscr.ActivityId != null)
            {
                IMessageStartEventHandler handler = new MessageStartEventHandler();

                var procInst = await handler.ExecuteAsync(this.context, 
                    eventSubscr, 
                    messageData);

                return procInst;
            }

            return null;
        }

        //public IActivityInstanceQuery CreateActivityQuery()
        //    => this.instanceStore.CreateActivityQuery();

        //public virtual ITokenQuery CreateTokenQuery()
        //    => this.instanceStore.CreateTokenQuery();

        //public virtual void Trigger(long tokenId)
        //{
        //    var token = this.instanceStore.FindToken(tokenId);
        //    token.Signal(Context.Current);
        //}

        public virtual IProcessInstanceBuilder CreateProcessInstanceBuilder()
        {
            throw new NotImplementedException();
        }

        public virtual Task<ProcessInstance> StartProcessAsync(IProcessInstanceBuilder builder)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> GetActiveTaskCountAsync(long tokenId)
        {
            var query = this.db.Tasks.Where(x => x.Token.Id == tokenId);
            return this.db.CountAsync(query);
        }

        public ITokenQuery CreateTokenQuery()
        {
            throw new NotImplementedException();
        }

        public virtual Task SaveAsync(ProcessInstance processInstance)
        {
            return this.db.UpdateAsync(processInstance);
        }

        public virtual Task<ProcessInstance> FindAsync(long processInstanceId)
            => this.db.FindAsync<ProcessInstance>(processInstanceId);

        public virtual async Task<IDictionary<string, object>> GetVariablesAsync(long processInstanceId, string[] variableNames = null)
        {
            var items = await this.GetVariableInstancesAsync(processInstanceId, variableNames);
            var map = new Dictionary<string, object>();
            foreach(var item in items)
                map.Add(item.Name, item.GetValue());

            return map;
        }

        public virtual async Task<IList<Variable>> GetVariableInstancesAsync(long processInstanceId, 
            string[] variableNames = null)
        {
            var query = this.db.ProcessInstances
                .Where(x => x.Id == processInstanceId
                    && x.Variables.Any(y => variableNames.Contains(y.Name)))
                    .Select(x => x.Variables);

            var items = await this.db.QuerySingleAsync(query);
            return items.ToList();
        }

        public virtual async Task SetVariablesAsync(long processInstanceId, IDictionary<string, object> variables)
        {
            var names = variables.Keys.ToArray();
            var variableInstances = await this.GetVariableInstancesAsync(processInstanceId, names);

            object value = null;
            foreach (var item in variableInstances)
            {
                value = null;
                if (variables.TryGetValue(item.Name, out value))
                    item.SetValue(value);
            }

            //async this.instanceStore.UpdateAsync()
        }

        public virtual async Task SetNameAsync(long processInstanceId, string name)
        {
            var pi = await this.FindAsync(processInstanceId);
            pi.Name = name;

            await this.db.UpdateAsync(pi);
        }

        public Task SuspendAsync(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public Task ResumeAsync(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public Task AbortAsync(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }
    }
}
