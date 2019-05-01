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

        public virtual IProcessInstanceQuery CreateInstanceQuery()
        {
            return new ProcessInstanceQuery(this.session);
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
                throw new ObjectNotFoundException(nameof(ProcessDefinition));

            var builder = this.CreateInstanceBuilder()
                .SetInitiator(context.UserId)
                .SetProcessDefinition(processDefinition)
                .SetVariables(variables);

            return await this.StartProcessAsync(builder);
        }

        #region IdentityLinks Management

        public virtual Task<IList<IdentityLink>> GetIdentityLinksAsync(long processInstanceId)
        {
            var query = this.session.IdentityLinks;

            query = this.session.Fetch(query, x => x.User);
            query = this.session.Fetch(query, x => x.Group);
            query = query.Where(x => x.ProcessInstance.Id == processInstanceId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<IList<IdentityLink>> AddUserLinksAsync(long processInstanceId, IEnumerable<int> userIds, string type)
        {
            if (userIds == null)
                throw new ArgumentNullException(nameof(userIds));

            if (!userIds.Any())
                throw new ArgumentException(nameof(userIds));

            var processInstance = await this.FindAsync(processInstanceId);
            if (processInstance == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            var users = await this.context.IdentityManager
                .GetUsersAsync(userIds.ToArray());

            var list = new List<IdentityLink>();

            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    var item = new IdentityLink();
                    item.ProcessInstance = processInstance;
                    item.User = user;
                    item.Type = type;
                    item.Created = Clock.Now;

                    list.Add(item);
                }

                await this.session.SaveRangeAsync(list);
                await this.session.FlushAsync();
            }

            return list;
        }

        public virtual async Task<IList<IdentityLink>> AddGroupLinksAsync(long processInstanceId,
            IEnumerable<int> groupIds, string type)
        {
            if (groupIds == null)
                throw new ArgumentNullException(nameof(groupIds));

            if (!groupIds.Any())
                throw new ArgumentException(nameof(groupIds));

            var processInstance = await this.FindAsync(processInstanceId);
            if (processInstance == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            var groups = await this.context.IdentityManager
                .GetGroupsAsync(groupIds.ToArray());

            var list = new List<IdentityLink>();
            if (groups.Count > 0)
            {
                foreach (var group in groups)
                {
                    var item = new IdentityLink();
                    item.ProcessInstance = processInstance;
                    item.Group = group;
                    item.Created = Clock.Now;

                    list.Add(item);
                }

                await this.session.SaveRangeAsync(list);
                await this.session.FlushAsync();
            }

            return list;
        }

        public virtual async Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds)
        {
            //check process-instance state.

            //fetch items to be deleted.
            var query = this.session.IdentityLinks
                .Where(x => x.ProcessInstance.Id == processInstanceId
                && identityLinkIds.Contains(x.Id));

            var items = await this.session.QueryMultipleAsync(query);
            if (items.Count > 0)
            {
                await this.session.RemoveRangeAsync(items);
                await this.session.FlushAsync();
            }
        }

        #endregion

        #region Start new ProcessInstance APIs

        public virtual async Task<ProcessInstance> StartProcessAsync(IProcessInstanceBuilder builder)
        {
            var pi = await builder.BuildAsync();

            var initialNodes = builder.InitialNodes;

            if (initialNodes.Count == 0)
                throw new RuntimeException($"The process '{pi.ProcessDefinition.Key}' does not cantains any start nodes.");

            if (initialNodes.Count > 1)
                throw new NotSupportedException("Multiple startEvents not supported.");

            //var tokens = new List<Token>();
            //foreach (var initialNode in initialNodes)
            //{
            var token = new Token(pi);
            token.Node = initialNodes[0];
            pi.Tokens.Add(token);
            //tokens.Add(token);
            //}

            //Update process-instance status.
            pi.StartTime = Clock.Now;
            pi.State = ExecutionState.Active;
            pi.LastStateTime = pi.StartTime.Value;

            //Save tokens.
            await this.context.DbSession.FlushAsync();

            //foreach (var token in tokens)
            //{
            //    //Check if process-instance isEnded.
            //    if (pi.IsEnded)
            //        break;

            var executionContext = ExecutionContext.Create(this.context, token);

            //fire processStartEvent.
            await this.context.Engine.ProcessEventListener.StartedAsync(executionContext);

            await executionContext.StartAsync();
            //}

            return pi;
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

        #endregion

        //public IActivityInstanceQuery CreateActivityQuery()
        //    => this.instanceStore.CreateActivityQuery();

        //public virtual ITokenQuery CreateTokenQuery()
        //    => this.instanceStore.CreateTokenQuery();

        //public virtual void Trigger(long tokenId)
        //{
        //    var token = this.instanceStore.FindToken(tokenId);
        //    token.Signal(Context.Current);
        //}

        public virtual IProcessInstanceBuilder CreateInstanceBuilder()
            => new ProcessInstanceBuilder(this);

        public virtual Task<int> GetActiveTaskCountAsync(long tokenId)
        {
            var query = this.session.Tasks.Where(x => x.Token.Id == tokenId);
            return this.session.CountAsync(query);
        }

        public virtual Task<ProcessInstance> FindAsync(long processInstanceId)
            => this.session.FindAsync<ProcessInstance>(processInstanceId);

        #region Variables Management

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
            var query = this.session.Query<Variable>()
                .Where(x => x.ProcessInstance.Id == processInstanceId
                    && (variableNames == null || variableNames.Contains(x.Name)));

            var items = await this.session.QueryMultipleAsync(query);
            return items;
        }

        public virtual async Task SetVariablesAsync(long processInstanceId, 
            IDictionary<string, object> variables)
        {
            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            if (variables.Count == 0)
                return;

            var processInstance = await this.CreateInstanceQuery()
                .FetchVariables()
                .SetId(processInstanceId)
                .SingleAsync();

            if (processInstance == null)
                throw new RuntimeException("The specified process instance does not exists.");

            if(processInstance.IsEnded)
                throw new RuntimeException("The specified process instance is already ended.");

            var em = variables.GetEnumerator();
            object value = null;
            string name = null;

            while (em.MoveNext())
            {
                name = em.Current.Key;
                value = em.Current.Value;

                if(value != null) //ignore null variables.
                    processInstance.SetVariable(name, value);
            }

            await this.session.FlushAsync();
        }

        #endregion

        #region Instance Attributes

        public virtual async Task SetKeyAsync(long processInstanceId, string key)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Key = key;

            await this.session.FlushAsync();
        }

        public virtual async Task SetNameAsync(long processInstanceId, string name)
        {
            var pi = await this.FindAsync(processInstanceId);
            pi.Name = name;

            await this.session.FlushAsync();
        }

        #endregion

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

        #region Comments Management

        public virtual Task<IList<Comment>> GetCommentsAsync(long processInstanceId)
        {
            var query = this.session.Query<Comment>()
                .Where(x => x.ProcessInstance.Id == processInstanceId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<Comment> AddCommentAsync(long processInstanceId, string comment)
        {
            var procInst = await this.FindAsync(processInstanceId);
            if (procInst == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            var item = new Comment();
            item.ProcessInstance = procInst;
            item.User = await this.context.IdentityManager.FindUserByIdAsync(context.UserId);
            item.Body = comment;
            item.Created = Clock.Now;

            await this.session.SaveAsync(item);
            await this.session.FlushAsync();

            return item;
        }

        public virtual async Task RemoveCommentAsync(long commentId)
        {
            var query = this.session.Query<Comment>()
                .Where(x => x.Id == commentId);

            var item = await this.session.QuerySingleAsync(query);
            if (item == null)
                throw new ObjectNotFoundException(nameof(Comment));

            await this.session.RemoveAsync(item);
            await this.session.FlushAsync();
        }

        #endregion
    }
}
