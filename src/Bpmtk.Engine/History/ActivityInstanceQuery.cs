using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.History
{
    public class ActivityInstanceQuery : IActivityInstanceQuery
    {
        protected bool fetchVariables;
        protected bool fetchSubProcessInstance;
        protected bool fetchProcessInstance;
        protected bool fetchIdentityLinks;

        protected long? id;
        protected bool? isMIRoot;
        protected string activityType;
        protected string name;
        protected string key;
        protected string processDefinitionKey;
        protected long? processInstanceId;
        protected string activityId;
        protected int? deploymentId;
        protected ExecutionState? state;
        protected ExecutionState[] anyStates;

        public ActivityInstanceQuery(IDbSession session)
        {
            Session = session;
        }

        public virtual IDbSession Session { get; }

        protected IQueryable<ActivityInstance> CreateNativeQuery()
        {
            var query = this.Session.ActivityInstances;

            if (this.fetchVariables)
                query = this.Session.Fetch(query, x => x.Variables);

            if (this.fetchProcessInstance)
                query = this.Session.Fetch(query, x => x.ProcessInstance);

            if (this.fetchIdentityLinks)
                query = this.Session.Fetch(query, x => x.IdentityLinks);

            if (this.fetchSubProcessInstance)
                query = this.Session.Fetch(query, x => x.SubProcessInstance);

            if (this.id.HasValue)
                return query.Where(x => x.Id == this.id);

            if (this.isMIRoot != null)
                query = query.Where(x => x.IsMIRoot == this.isMIRoot);

            if (this.state != null)
                query = query.Where(x => x.State == this.state);
            else if (this.anyStates != null && this.anyStates.Length > 0)
            {
                query = query.Where(x => this.anyStates.Contains(x.State));
            }

            if (this.activityId != null)
                query = query.Where(x => x.ActivityId == this.activityId);

            if (this.processInstanceId != null)
                query = query.Where(x => x.ProcessInstance.Id == this.processInstanceId);

            if (this.name != null)
                query = query.Where(x => x.Name.Contains(this.name));

            return query;
        }

        public virtual Task<int> CountAsync()
        {
            return this.Session.CountAsync(this.CreateNativeQuery());
        }

        public virtual IActivityInstanceQuery FetchIdentityLinks()
        {
            this.fetchIdentityLinks = true;

            return this;
        }

        public virtual IActivityInstanceQuery FetchSubProcessInstance()
        {
            this.fetchSubProcessInstance = true;

            return this;
        }

        public virtual IActivityInstanceQuery FetchProcessInstance()
        {
            this.fetchProcessInstance = true;

            return this;
        }

        public virtual IActivityInstanceQuery FetchVariables()
        {
            this.fetchVariables = true;

            return this;
        }

        public virtual Task<IList<ActivityInstance>> ListAsync(int page = 1, int pageSize = 20)
        {
            if (page < 1)
                page = 1;

            var query = this.CreateNativeQuery()
                .OrderByDescending(x => x.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ActivityInstance>> ListAsync()
        {
            return this.Session.QueryMultipleAsync(this.CreateNativeQuery());
        }

        public virtual IActivityInstanceQuery SetActivityType(string activityType)
        {
            this.activityType = activityType;

            return this;
        }

        public virtual IActivityInstanceQuery SetDeploymentId(int deploymentId)
        {
            this.deploymentId = deploymentId;

            return this;
        }

        public virtual IActivityInstanceQuery SetActivityId(string activityId)
        {
            this.activityId = activityId;

            return this;
        }

        public virtual IActivityInstanceQuery SetId(long id)
        {
            this.id = id;

            return this;
        }

        public virtual IActivityInstanceQuery SetIsMIRoot(bool isMIRoot)
        {
            this.isMIRoot = isMIRoot;

            return this;
        }

        public virtual IActivityInstanceQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual IActivityInstanceQuery SetProcessInstanceId(long processInstanceId)
        {
            this.processInstanceId = processInstanceId;

            return this;
        }

        public virtual IActivityInstanceQuery SetStartTimeFrom(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public virtual IActivityInstanceQuery SetStartTimeTo(DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public virtual IActivityInstanceQuery SetState(ExecutionState state)
        {
            this.state = state;

            return this;
        }

        public virtual IActivityInstanceQuery SetStateAny(params ExecutionState[] stateArray)
        {
            this.anyStates = stateArray;

            return this;
        }

        public virtual Task<ActivityInstance> SingleAsync()
        {
            return this.Session.QuerySingleAsync(this.CreateNativeQuery());
        }
    }
}
