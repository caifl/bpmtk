using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Runtime
{
    public class RuntimeManager : IRuntimeManager
    {
        private readonly Context context;
        private readonly IDbSession session;
        protected readonly IDeploymentManager deploymentManager;

        public virtual IQueryable<ProcessInstance> ProcessInstances => this.session.ProcessInstances;

        public virtual IQueryable<Token> Tokens => this.session.Tokens;

        public RuntimeManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;
            this.deploymentManager = context.DeploymentManager;
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
            var query = this.session.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct();

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<ProcessInstance> StartProcessByKeyAsync(string processDefinitionKey,
            IDictionary<string, object> variables)
        {
            var processDefinition = await this.deploymentManager.FindProcessDefinitionByKeyAsync(processDefinitionKey);
            if (processDefinition == null)
                throw new KeyNotFoundException("The specified process-definition was not found.");

            var builder = this.CreateProcessInstanceBuilder()
                .SetProcessDefinition(processDefinition)
                .SetVariables(variables);

            return await this.StartProcessAsync(builder);

            //var model = await this.deploymentManager.GetBpmnModelAsync(processDefinition.DeploymentId);
            //var process = model.GetProcess(processDefinition.Key);
            //if (process == null)
            //    throw new RuntimeException($"The BPMN model not contains process '{processDefinition.Key}'");

            //var initialNode = process.InitialNode;
            //if (initialNode == null)
            //    throw new RuntimeException($"No start element found for process definition " + processDefinition.Key) ;

            //var pi = new ProcessInstance();
            //pi.ProcessDefinition = processDefinition;

            //var context = Context.Current;

            //pi.Initiator = new User() { Id = context.UserId };

            ////pi.InitializeContext(context, variables);

            //await this.db.SaveAsync(pi);

            //pi.Start(context, initialNode);
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

            await this.session.UpdateAsync(inst);
        }

        public virtual async Task SetProcessInstanceNameAsync(long processInstanceId, 
            string name)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Name = name;

            await this.session.UpdateAsync(inst);
        }

        public virtual async Task<ProcessInstance> StartProcessByMessageAsync(string messageName, 
            IDictionary<string, object> messageData = null)
        {
            var query = this.session.EventSubscriptions
                .Where(x => x.EventType == "message"
                    && x.EventName == messageName
                  );

            var eventSubscr = await this.session.QuerySingleAsync(query);
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
            => new ProcessInstanceBuilder(this);

        public virtual async Task<ProcessInstance> StartProcessAsync(IProcessInstanceBuilder builder)
        {
            var pi = await builder.BuildAsync();

            var initialNodes = builder.InitialNodes;
            var tokens = new List<Token>();
            foreach (var initialNode in initialNodes)
            {
                var token = new Token(pi);
                token.Node = initialNode;
                pi.Tokens.Add(token);
                tokens.Add(token);
            }

            //Update process-instance status.
            pi.StartTime = Clock.Now;
            pi.State = ExecutionState.Active;
            pi.LastStateTime = pi.StartTime.Value;

            //Save tokens.
            await this.context.DbSession.FlushAsync();

            //fire processInstanceStartEvent.

            foreach (var token in tokens)
            {
                //Check if process-instance isEnded.
                if (pi.IsEnded)
                    break;

                var executionContext = ExecutionContext.Create(this.context, token);
                await executionContext.StartAsync();
            }

            return pi;
        }

        public virtual Task<int> GetActiveTaskCountAsync(long tokenId)
        {
            var query = this.session.Tasks.Where(x => x.Token.Id == tokenId);
            return this.session.CountAsync(query);
        }

        public ITokenQuery CreateTokenQuery()
        {
            throw new NotImplementedException();
        }

        public virtual Task SaveAsync(ProcessInstance processInstance)
        {
            return this.session.UpdateAsync(processInstance);
        }

        public virtual Task<ProcessInstance> FindAsync(long processInstanceId)
            => this.session.FindAsync<ProcessInstance>(processInstanceId);

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
            var query = this.session.ProcessInstances
                .Where(x => x.Id == processInstanceId
                    && x.Variables.Any(y => variableNames.Contains(y.Name)))
                    .Select(x => x.Variables);

            var items = await this.session.QuerySingleAsync(query);
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

            await this.session.UpdateAsync(pi);
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
