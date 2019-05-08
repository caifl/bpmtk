using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Variables;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Runtime
{
    public class RuntimeManager : IRuntimeManager
    {
        protected CompositeProcessEventListener compositeProcessEventListener;

        private readonly Context context;
        private readonly IDbSession session;
        protected DeploymentManager deploymentManager;

        public RuntimeManager(Context context)
        {
            this.context = context;
            this.deploymentManager = context.DeploymentManager;
            this.session = context.DbSession;

            var options = context.Engine.Options;
            this.compositeProcessEventListener = new CompositeProcessEventListener(options.ProcessEventListeners);
        }

        public virtual Context Context
        {
            get => this.context;
        }

        public virtual ProcessInstanceQuery CreateInstanceQuery()
        {
            return new ProcessInstanceQuery(this.session);
        }

        IProcessInstanceQuery IRuntimeManager.CreateInstanceQuery() => this.CreateInstanceQuery();

        public virtual IList<string> GetActiveActivityIds(long processInstanceId)
        {
            var query = this.session.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct();

            return query.ToList();
        }

        public virtual Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId)
        {
            var query = this.session.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive && x.ActivityId != null)
                .Select(x => x.ActivityId)
                .Distinct();

            return this.session.QueryMultipleAsync(query);
        }

        public virtual IList<Token> GetActiveTokens(long processInstanceId)
        {
            var query = this.session.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive);

            return query.ToList();
        }

        public virtual Task<IList<Token>> GetActiveTokensAsync(long processInstanceId)
        {
            var query = this.session.Tokens
                .Where(x => x.ProcessInstance.Id == processInstanceId && x.IsActive);

            return this.session.QueryMultipleAsync(query);
        }

        IList<IToken> IRuntimeManager.GetActiveTokens(long processInstanceId)
            => new List<IToken>(this.GetActiveTokens(processInstanceId));

        async Task<IList<IToken>> IRuntimeManager.GetActiveTokensAsync(long processInstanceId)
            => new List<IToken>(await this.GetActiveTokensAsync(processInstanceId));

        public virtual void Trigger(long tokenId, 
            IDictionary<string, object> variables = null)
        {
            var token = this.session.Find<Token>(tokenId);
            if (token == null)
                throw new ObjectNotFoundException(nameof(Token));

            var ecm = this.context.ExecutionContextManager;
            var executionContext = ecm.GetOrCreate(token);
            executionContext.Trigger(null, variables);
        }

        public virtual async Task TriggerAsync(long tokenId, IDictionary<string, object> variables = null)
        {
            var token = await this.session.FindAsync<Token>(tokenId);
            if (token == null)
                throw new ObjectNotFoundException(nameof(Token));

            var ecm = this.context.ExecutionContextManager;
            var executionContext = ecm.GetOrCreate(token);
            executionContext.Trigger(null, variables);
        }

        #region IdentityLinks Management

        public virtual Task<IList<IdentityLink>> GetIdentityLinksAsync(long processInstanceId)
        {
            var query = this.session.IdentityLinks;
            query = query.Where(x => x.ProcessInstance.Id == processInstanceId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<IList<IdentityLink>> AddUserLinksAsync(long processInstanceId, IEnumerable<string> userIds, string type)
        {
            if (userIds == null)
                throw new ArgumentNullException(nameof(userIds));

            if (!userIds.Any())
                throw new ArgumentException(nameof(userIds));

            var processInstance = this.Find(processInstanceId);
            if (processInstance == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            //var users = this.context.IdentityManager
            //    .GetUsersAsync(userIds.ToArray());

            var list = new List<IdentityLink>();
            foreach (var userId in userIds)
            {
                var item = new IdentityLink();
                item.ProcessInstance = processInstance;
                item.UserId = userId;
                item.Type = type;
                item.Created = Clock.Now;

                list.Add(item);
            }

            await this.session.SaveRangeAsync(list);
            await this.session.FlushAsync();

            return list;
        }

        public virtual async Task<IList<IdentityLink>> AddGroupLinksAsync(long processInstanceId,
            IEnumerable<string> groupIds, string type)
        {
            if (groupIds == null)
                throw new ArgumentNullException(nameof(groupIds));

            if (!groupIds.Any())
                throw new ArgumentException(nameof(groupIds));

            var processInstance = this.Find(processInstanceId);
            if (processInstance == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            //var groups = this.context.IdentityManager
            //    .GetGroupsAsync(groupIds.ToArray());

            var list = new List<IdentityLink>();
            //if (groups.Count > 0)
            //{
            foreach (var group in groupIds)
            {
                var item = new IdentityLink();
                item.ProcessInstance = processInstance;
                item.GroupId = group;
                item.Created = Clock.Now;

                list.Add(item);
            }

            await this.session.SaveRangeAsync(list);
            await this.session.FlushAsync();
            //}

            return list;
        }

        public virtual async Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds)
        {
            //check process-instance state.

            //fetch items to be deleted.
            var query = this.session.IdentityLinks
                .Where(x => x.ProcessInstance.Id == processInstanceId
                && identityLinkIds.Contains(x.Id));

            var items = query.ToList();
            if (items.Count > 0)
            {
                this.session.RemoveRange(items);
                await this.session.FlushAsync();
            }
        }

        #endregion

        #region Start new ProcessInstance APIs

        async Task<IProcessInstance> IRuntimeManager.StartProcessAsync(IProcessInstanceBuilder builder)
            => await this.StartProcessAsync((ProcessInstanceBuilder)builder);

        async Task<IProcessInstance> IRuntimeManager.StartProcessByMessageAsync(string messageName,
            IDictionary<string, object> messageData)
            => await this.StartProcessByMessageAsync(messageName, messageData);

        //IProcessInstance IRuntimeManager.StartProcessByKey(string processDefinitionKey,
        //    IDictionary<string, object> variables)
        //    => this.StartProcessByKey(processDefinitionKey, variables);

        async Task<IProcessInstance> IRuntimeManager.StartProcessByKeyAsync(string processDefinitionKey,
            IDictionary<string, object> variables)
            => await this.StartProcessByKeyAsync(processDefinitionKey, variables);

        //public virtual ProcessInstance StartProcessByKey(string processDefinitionKey,
        //    IDictionary<string, object> variables)
        //{
        //    var processDefinition = this.deploymentManager.FindProcessDefinitionByKey(processDefinitionKey);
        //    if (processDefinition == null)
        //        throw new ObjectNotFoundException(nameof(ProcessDefinition));

        //    var builder = this.CreateInstanceBuilder()
        //        .SetInitiator(context.UserId)
        //        .SetProcessDefinition(processDefinition)
        //        .SetVariables(variables);

        //    return this.StartProcess(builder);
        //}

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

        //public virtual ProcessInstance StartProcess(ProcessInstanceBuilder builder)
        //{
        //    if (builder == null)
        //        throw new ArgumentNullException(nameof(builder));

        //    var pi = builder.Build();

        //    var date = pi.Created;

        //    //Update process-instance status.
        //    pi.StartTime = date;
        //    pi.State = ExecutionState.Active;
        //    pi.LastStateTime = date;

        //    //Save changes.
        //    this.session.Flush();

        //    //Get root-token.
        //    var rootToken = pi.Token;

        //    var ecm = this.context.ExecutionContextManager;
        //    var executionContext = ecm.Create(rootToken);
        //    executionContext.Start();

        //    return pi;
        //}

        public virtual async Task<ProcessInstance> StartProcessAsync(ProcessInstanceBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var pi = await builder.BuildAsync();

            //Update process-instance status.
            pi.StartTime = Clock.Now;
            pi.State = ExecutionState.Active;
            pi.LastStateTime = pi.StartTime.Value;

            //Save tokens.
            await this.session.FlushAsync();

            var rootToken = pi.Token;

            var ecm = this.context.ExecutionContextManager;
            var executionContext = ecm.Create(rootToken);
            executionContext.Start();

            return pi;
        }

        public virtual CompositeProcessEventListener GetCompositeProcessEventListener()
            => this.compositeProcessEventListener;

        public virtual async Task<ProcessInstance> StartProcessByMessageAsync(string messageName, 
            IDictionary<string, object> messageData = null)
        {
            if (string.IsNullOrEmpty(messageName))
                throw new ArgumentException("message", nameof(messageName));

            var eventSubscr = deploymentManager.FindEventSubscriptionByMessage(messageName);
            if (eventSubscr == null)
                throw new RuntimeException($"The message '{messageName}' event handler does not exists.");

            if(eventSubscr.ProcessDefinition != null
                && eventSubscr.ActivityId != null)
            {
                IMessageStartEventHandler handler = new MessageStartEventHandler();

                var procInst = await handler.ExecuteAsync(this.context, 
                    eventSubscr, 
                    messageData);

                return (ProcessInstance)procInst;
            }

            return null;
        }

        #endregion

        public virtual ProcessInstanceBuilder CreateInstanceBuilder()
            => new ProcessInstanceBuilder(this.context);

        IProcessInstanceBuilder IRuntimeManager.CreateInstanceBuilder() => this.CreateInstanceBuilder();

        public virtual int GetActiveTaskCount(long tokenId)
        {
            var query = this.session.Tasks.Where(x => x.Token.Id == tokenId
                && x.State == Tasks.TaskState.Active);

            return query.Count();
        }

        public virtual ProcessInstance Find(long processInstanceId)
            => this.session.Find<ProcessInstance>(processInstanceId);

        public virtual Task<ProcessInstance> FindAsync(long processInstanceId)
            => this.session.FindAsync<ProcessInstance>(processInstanceId);

        IProcessInstance IRuntimeManager.Find(long processInstanceId) 
            => this.Find(processInstanceId);

        async Task<IProcessInstance> IRuntimeManager.FindAsync(long processInstanceId)
            => await this.FindAsync(processInstanceId);

        #region Variables Management

        public virtual IDictionary<string, object> GetVariables(long processInstanceId, string[] variableNames = null)
        {
            var items = this.GetVariableInstances(processInstanceId, variableNames);
            var map = new Dictionary<string, object>();
            foreach(var item in items)
                map.Add(item.Name, item.GetValue());

            return map;
        }

        public virtual async Task<IDictionary<string, object>> GetVariablesAsync(long processInstanceId, string[] variableNames = null)
        {
            var items = await this.GetVariableInstancesAsync(processInstanceId, variableNames);
            var map = new Dictionary<string, object>();
            foreach (var item in items)
                map.Add(item.Name, item.GetValue());

            return map;
        }

        public virtual async Task<IList<IVariable>> GetVariableInstancesAsync(long processInstanceId,
            string[] variableNames = null)
        {
            var query = this.session.Query<Variable>()
                .Where(x => x.ProcessInstance.Id == processInstanceId
                    && (variableNames == null || variableNames.Contains(x.Name)));

            var items = await this.session.QueryMultipleAsync(query);
            return new List<IVariable>(items);
        }

        public virtual IList<IVariable> GetVariableInstances(long processInstanceId, 
            string[] variableNames = null)
        {
            var query = this.session.Query<Variable>()
                .Where(x => x.ProcessInstance.Id == processInstanceId
                    && (variableNames == null || variableNames.Contains(x.Name)));

            return query.ToList<IVariable>();
        }

        public virtual void SetVariables(long processInstanceId, 
            IDictionary<string, object> variables)
        {
            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            if (variables.Count == 0)
                return;

            var processInstance = (ProcessInstance)this.CreateInstanceQuery()
                .FetchVariables()
                .SetId(processInstanceId)
                .Single();

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

            this.session.Flush();
        }

        #endregion

        #region Instance Attributes

        public virtual async Task<IProcessInstance> SetKeyAsync(long processInstanceId, string key)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Key = key;

            await this.session.FlushAsync();
            return inst;
        }

        public virtual async Task<IProcessInstance> SetNameAsync(long processInstanceId, string name)
        {
            var inst = await this.FindAsync(processInstanceId);
            inst.Name = name;

            await this.session.FlushAsync();
            return inst;
        }

        #endregion

        public void Suspend(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public void Resume(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public void Abort(long processInstanceId, string comment = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(long processInstanceId, string comment = null)
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
            var procInst = this.Find(processInstanceId);
            if (procInst == null)
                throw new ObjectNotFoundException(nameof(ProcessInstance));

            var item = new Comment();
            item.ProcessInstance = procInst;
            item.UserId = context.UserId;
            item.Body = comment;
            item.Created = Clock.Now;

            await this.session.SaveAsync(item);
            await this.session.FlushAsync();

            return item;
        }

        public virtual void RemoveComment(long commentId)
        {
            var query = this.session.Query<Comment>()
                .Where(x => x.Id == commentId);

            var item = query.SingleOrDefault();
            if (item == null)
                throw new ObjectNotFoundException(nameof(Comment));

            this.session.Remove(item);
            this.session.Flush();
        }

        #endregion
    }
}
