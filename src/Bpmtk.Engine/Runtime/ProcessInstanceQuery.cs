using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Runtime
{
    public class ProcessInstanceQuery : IProcessInstanceQuery
    {
        protected bool fetchVariables;
        protected bool fetchProcessDefinition;
        protected bool fetchIdentityLinks;

        protected long? id;
        protected string category;
        protected string name;
        protected string key;
        protected string processDefinitionKey;
        protected int? processDefinitionId;
        protected string initiator;
        protected int? deploymentId;
        protected ExecutionState? state;
        protected ExecutionState[] anyStates;

        public ProcessInstanceQuery(IDbSession session)
        {
            Session = session;
        }

        public virtual IDbSession Session { get; }

        protected IQueryable<ProcessInstance> CreateNativeQuery()
        {
            var query = this.Session.ProcessInstances;

            if (this.fetchVariables)
                query = this.Session.Fetch(query, x => x.Variables);

            if (this.fetchProcessDefinition)
                query = this.Session.Fetch(query, x => x.ProcessDefinition);

            if (this.fetchIdentityLinks)
                query = this.Session.Fetch(query, x => x.IdentityLinks);

            if (this.id.HasValue)
                return query.Where(x => x.Id == this.id);

            if (this.key != null)
                query = query.Where(x => x.Key == this.key);

            if (this.state != null)
                query = query.Where(x => x.State == this.state);
            else if (this.anyStates != null && this.anyStates.Length > 0)
            {
                query = query.Where(x => this.anyStates.Contains(x.State));
            }

            if (this.initiator != null)
                query = query.Where(x => x.Initiator == this.initiator);

            if (this.processDefinitionId != null)
                query = query.Where(x => x.ProcessDefinition.Id == this.processDefinitionId);
            else
            {
                if (this.processDefinitionKey != null)
                    query = query.Where(x => x.ProcessDefinition.Key == this.processDefinitionKey);

                if (this.category != null)
                    query = query.Where(x => x.ProcessDefinition.Category == this.category);

                if (this.deploymentId != null)
                    query = query.Where(x => x.ProcessDefinition.Deployment.Id == this.deploymentId);
            }

            if (this.name != null)
                query = query.Where(x => x.Name.Contains(this.name));

            return query;
        }

        public virtual Task<int> CountAsync()
        {
            return this.Session.CountAsync(this.CreateNativeQuery());
        }

        public virtual IProcessInstanceQuery FetchIdentityLinks()
        {
            this.fetchIdentityLinks = true;

            return this;
        }

        public virtual IProcessInstanceQuery FetchProcessDefinition()
        {
            this.fetchProcessDefinition = true;

            return this;
        }

        public virtual IProcessInstanceQuery FetchVariables()
        {
            this.fetchVariables = true;

            return this;
        }

        public virtual Task<IList<ProcessInstance>> ListAsync(int page = 1, int pageSize = 20)
        {
            if (page < 1)
                page = 1;

            var query = this.CreateNativeQuery()
                .OrderByDescending(x => x.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ProcessInstance>> ListAsync()
        {
            return this.Session.QueryMultipleAsync(this.CreateNativeQuery());
        }

        public virtual IProcessInstanceQuery SetCategory(string category)
        {
            this.category = category;

            return this;
        }

        public virtual IProcessInstanceQuery SetDeploymentId(int deploymentId)
        {
            this.deploymentId = deploymentId;

            return this;
        }

        public virtual IProcessInstanceQuery SetInitiator(string initiator)
        {
            this.initiator = initiator;

            return this;
        }

        public virtual IProcessInstanceQuery SetId(long id)
        {
            this.id = id;

            return this;
        }

        public virtual IProcessInstanceQuery SetKey(string key)
        {
            this.key = key;

            return this;
        }

        public virtual IProcessInstanceQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual IProcessInstanceQuery SetProcessDefinitionId(int processDefinitionId)
        {
            this.processDefinitionId = processDefinitionId;

            return this;
        }

        public virtual IProcessInstanceQuery SetProcessDefinitionKey(string processDefinitionKey)
        {
            this.processDefinitionKey = processDefinitionKey;

            return this;
        }

        public virtual IProcessInstanceQuery SetStartTimeFrom(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public virtual IProcessInstanceQuery SetStartTimeTo(DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public virtual IProcessInstanceQuery SetState(ExecutionState state)
        {
            this.state = state;

            return this;
        }

        public virtual IProcessInstanceQuery SetStateAny(params ExecutionState[] stateArray)
        {
            this.anyStates = stateArray;

            return this;
        }

        public virtual ProcessInstance Single()
        {
            return this.CreateNativeQuery().SingleOrDefault();
        }

        public virtual Task<ProcessInstance> SingleAsync()
        {
            return this.Session.QuerySingleAsync(this.CreateNativeQuery());
        }

        IProcessInstance IProcessInstanceQuery.Single()
            => this.Single();

        async Task<IProcessInstance> IProcessInstanceQuery.SingleAsync()
            => await this.SingleAsync();

        async Task<IList<IProcessInstance>> IProcessInstanceQuery.ListAsync()
        {
            var list = await this.ListAsync();
            return list.ToList<IProcessInstance>();
        }

        //async Task<IList<IProcessInstance>> IProcessInstanceQuery.ListAsync(int count)
        //{
        //    var list = this.ListAsync(count);
        //    return list.ToList<IProcessInstance>();
        //}

        async Task<IList<IProcessInstance>> IProcessInstanceQuery.ListAsync(int page, int pageSize)
        {
            var list = await this.ListAsync(page, pageSize);
            return list.ToList<IProcessInstance>();
        }
    }
}
