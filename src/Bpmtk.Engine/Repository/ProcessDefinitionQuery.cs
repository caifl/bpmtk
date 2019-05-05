using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Repository
{
    public class ProcessDefinitionQuery : IProcessDefinitionQuery
    {
        protected const string FetchLatestVersionOnlySQL = @"select p.* from bpm_proc_def p inner join 
(select `Key`, max(Version) as Version from bpm_proc_def
group by `Key`) x on p.`key` = x.`Key` and p.`Version` = x.`Version`";

        protected bool fetchDeployment;
        protected bool fetchIdentityLinks;
        protected bool fetchLastestVersionOnly;

        protected int? id;
        protected string category;
        protected string name;
        protected string key;
        protected int? version;
        protected int? deploymentId;
        protected string description;
        protected ProcessDefinitionState? state;
        protected ProcessDefinitionState[] anyStates;

        public ProcessDefinitionQuery(Context context)
        {
            Session = context.DbSession;
            Context = context;
        }

        public virtual IDbSession Session { get; }
        public Context Context { get; }

        protected IQueryable<ProcessDefinition> CreateNativeQuery()
        {
            IQueryable<ProcessDefinition> query = null;
            if (this.fetchLastestVersionOnly)
            {
                var sql = this.Context.Engine.GetValue<string>("fetchProcessLatestVersionSQL", FetchLatestVersionOnlySQL);
                query = this.Session.Query<ProcessDefinition>(sql);
            }
            else
            {
                query = this.Session.ProcessDefinitions;
            }

            if (this.fetchIdentityLinks)
                query = this.Session.Fetch(query, x => x.IdentityLinks);

            if (this.fetchDeployment)
                query = this.Session.Fetch(query, x => x.Deployment);

            if (this.fetchIdentityLinks)
                query = this.Session.Fetch(query, x => x.IdentityLinks);

            if (this.id.HasValue)
                return query.Where(x => x.Id == this.id);

            if (this.deploymentId != null)
                query = query.Where(x => x.DeploymentId == this.deploymentId);

            if (this.key != null)
                query = query.Where(x => x.Key == this.key);

            if (this.state != null)
                query = query.Where(x => x.State == this.state);
            else if (this.anyStates != null && this.anyStates.Length > 0)
                query = query.Where(x => this.anyStates.Contains(x.State));

            if (this.version != null)
                query = query.Where(x => x.Version == this.version);

            //if (this.processDefinitionId != null)
            //    query = query.Where(x => x.ProcessDefinition.Id == this.processDefinitionId);
            //else
            //{
            //    if (this.processDefinitionKey != null)
            //        query = query.Where(x => x.ProcessDefinition.Key == this.processDefinitionKey);

            //    if (this.category != null)
            //        query = query.Where(x => x.ProcessDefinition.Category == this.category);

            //    if (this.deploymentId != null)
            //        query = query.Where(x => x.ProcessDefinition.Deployment.Id == this.deploymentId);
            //}
            if (this.name != null)
                query = query.Where(x => x.Name.Contains(this.name));

            if (this.category != null)
                query = query.Where(x => x.Category == this.category);

            if (this.description != null)
                query = query.Where(x => x.Description.Contains(this.description));

            //if (this.fetchLastestVersionOnly)
            //{
            //    var groupByKey = query.GroupBy(x => x.Key)
            //        .Select(x => new
            //        {
            //            key = x.Key,
            //            version = x.Max(y => y.Version)
            //        });

            //    query = from item in query
            //             join version in groupByKey on new
            //             {
            //                 key = item.Key,
            //                 version = item.Version
            //             } equals version
            //             select item;
            //}

            return query;
        }

        protected virtual IQueryable<ProcessDefinition> Sort(IQueryable<ProcessDefinition> query)
        {
            return query.OrderByDescending(x => x.Created);
        }

        public virtual Task<int> CountAsync()
        {
            return this.Session.CountAsync(this.CreateNativeQuery());
        }

        public virtual IProcessDefinitionQuery FetchIdentityLinks()
        {
            this.fetchIdentityLinks = true;

            return this;
        }

        public virtual IProcessDefinitionQuery FetchDeployment()
        {
            this.fetchDeployment = true;

            return this;
        }

        public virtual IProcessDefinitionQuery FetchLatestVersionOnly()
        {
            this.fetchLastestVersionOnly = true;

            return this;
        }

        public virtual Task<IList<ProcessDefinition>> ListAsync(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            var query = this.Sort(this.CreateNativeQuery())
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ProcessDefinition>> ListAsync(int count)
        {
            var query = this.Sort( this.CreateNativeQuery() )
                .Take(count);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ProcessDefinition>> ListAsync()
        {
            return this.Session.QueryMultipleAsync(this.CreateNativeQuery());
        }

        public virtual IProcessDefinitionQuery SetCategory(string category)
        {
            this.category = category;

            return this;
        }

        public virtual IProcessDefinitionQuery SetDeploymentId(int deploymentId)
        {
            this.deploymentId = deploymentId;

            return this;
        }

        public virtual IProcessDefinitionQuery SetId(int id)
        {
            this.id = id;

            return this;
        }

        public virtual IProcessDefinitionQuery SetVersion(int version)
        {
            this.version = version;

            return this;
        }

        public virtual IProcessDefinitionQuery SetKey(string key)
        {
            this.key = key;

            return this;
        }

        public virtual IProcessDefinitionQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual IProcessDefinitionQuery SetStartTimeFrom(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public virtual IProcessDefinitionQuery SetStartTimeTo(DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public virtual IProcessDefinitionQuery SetState(ProcessDefinitionState state)
        {
            this.state = state;

            return this;
        }

        public virtual IProcessDefinitionQuery SetStateAny(params ProcessDefinitionState[] stateArray)
        {
            this.anyStates = stateArray;

            return this;
        }

        public virtual IProcessDefinitionQuery SetDescription(string description)
        {
            this.description = description;

            return this;
        }

        public virtual async Task<ProcessDefinition> SingleAsync()
        {
            return await this.Session.QuerySingleAsync(this.CreateNativeQuery());
        }

        async Task<IProcessDefinition> IProcessDefinitionQuery.SingleAsync()
            => await this.SingleAsync();

        async Task<IList<IProcessDefinition>> IProcessDefinitionQuery.ListAsync()
        {
            var list = await this.ListAsync();
            return list.ToList<IProcessDefinition>();
        }

        async Task<IList<IProcessDefinition>> IProcessDefinitionQuery.ListAsync(int count)
        {
            var list = await this.ListAsync(count);
            return list.ToList<IProcessDefinition>();
        }

        async Task<IList<IProcessDefinition>> IProcessDefinitionQuery.ListAsync(int page, int pageSize)
        {
            var list = await this.ListAsync(page, pageSize);
            return list.ToList<IProcessDefinition>();
        }
    }
}
