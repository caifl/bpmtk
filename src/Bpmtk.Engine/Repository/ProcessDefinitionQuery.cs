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
        protected IEnumerable<string> anyKeys;
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

        public virtual ProcessDefinitionQuery FetchIdentityLinks()
        {
            this.fetchIdentityLinks = true;

            return this;
        }

        public virtual ProcessDefinitionQuery FetchDeployment()
        {
            this.fetchDeployment = true;

            return this;
        }

        public virtual ProcessDefinitionQuery FetchLatestVersionOnly()
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

        public virtual ProcessDefinitionQuery SetCategory(string category)
        {
            this.category = category;

            return this;
        }

        public virtual ProcessDefinitionQuery SetDeploymentId(int deploymentId)
        {
            this.deploymentId = deploymentId;

            return this;
        }

        public virtual ProcessDefinitionQuery SetId(int id)
        {
            this.id = id;

            return this;
        }

        public virtual ProcessDefinitionQuery SetVersion(int version)
        {
            this.version = version;

            return this;
        }

        public virtual ProcessDefinitionQuery SetKey(string key)
        {
            this.key = key;

            return this;
        }

        public virtual ProcessDefinitionQuery SetKeyAny(IEnumerable<string> keys)
        {
            this.anyKeys = keys;

            return this;
        }

        public virtual ProcessDefinitionQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual ProcessDefinitionQuery SetStartTimeFrom(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        public virtual ProcessDefinitionQuery SetStartTimeTo(DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public virtual ProcessDefinitionQuery SetState(ProcessDefinitionState state)
        {
            this.state = state;

            return this;
        }

        public virtual ProcessDefinitionQuery SetStateAny(params ProcessDefinitionState[] stateArray)
        {
            this.anyStates = stateArray;

            return this;
        }

        public virtual ProcessDefinitionQuery SetDescription(string description)
        {
            this.description = description;

            return this;
        }

        public virtual ProcessDefinition Single()
        {
            return this.CreateNativeQuery().SingleOrDefault();
        }

        public virtual IList<ProcessDefinition> List()
        {
            return this.CreateNativeQuery().ToList();
        }

        IProcessDefinition IProcessDefinitionQuery.Single()
            => this.Single();

        public virtual Task<ProcessDefinition> SingleAsync()
            => this.Session.QuerySingleAsync(this.CreateNativeQuery());

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

        IProcessDefinitionQuery IProcessDefinitionQuery.SetId(int processDefinitionId)
            => this.SetId(processDefinitionId);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetDeploymentId(int deploymentId)
            => this.SetDeploymentId(deploymentId);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetKey(string key)
            => this.SetKey(key);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetKeyAny(IEnumerable<string> keys)
            => this.SetKeyAny(keys);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetCategory(string category)
            => this.SetCategory(category);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetState(ProcessDefinitionState state)
            => this.SetState(state);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetName(string name)
            => this.SetName(name);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetVersion(int version)
            => this.SetVersion(version);

        IProcessDefinitionQuery IProcessDefinitionQuery.SetDescription(string description)
            => this.SetDescription(description);

        IProcessDefinitionQuery IProcessDefinitionQuery.FetchDeployment()
            => this.FetchDeployment();

        IProcessDefinitionQuery IProcessDefinitionQuery.FetchIdentityLinks()
            => this.FetchIdentityLinks();

        IProcessDefinitionQuery IProcessDefinitionQuery.FetchLatestVersionOnly()
            => this.FetchLatestVersionOnly();

        IList<IProcessDefinition> IProcessDefinitionQuery.List()
            => new List<IProcessDefinition>(this.List());

        async Task<IProcessDefinition> IProcessDefinitionQuery.SingleAsync()
            => await this.SingleAsync();

        Task<int> IProcessDefinitionQuery.CountAsync()
            => this.CountAsync();
    }
}
