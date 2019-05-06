using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Repository
{
    public class DeploymentQuery : IDeploymentQuery
    {
        protected bool fetchUser;
        protected bool fetchModel;

        protected int? id;
        protected string category;
        protected string name;
        protected string key;
        protected string processDefinitionKey;
        protected int? packageId;
        protected string userId;
        protected DateTime? createdFrom;
        protected DateTime? createdTo;

        public DeploymentQuery(IDbSession session)
        {
            Session = session;
        }

        public virtual IDbSession Session { get; }

        protected IQueryable<Deployment> CreateNativeQuery()
        {
            var query = this.Session.Deployments;

            //if (this.fetchUser)
            //    query = this.Session.Fetch(query, x => x.User);

            if (this.fetchModel)
                query = this.Session.Fetch(query, x => x.Model);

            if (this.id.HasValue)
                return query.Where(x => x.Id == this.id);

            if (this.userId != null)
                query = query.Where(x => x.UserId == this.userId);

            if (this.packageId != null)
                query = query.Where(x => x.Package.Id == this.packageId);

            if (this.name != null)
                query = query.Where(x => x.Name.Contains(this.name));

            return query;
        }

        public virtual Task<int> CountAsync()
        {
            return this.Session.CountAsync(this.CreateNativeQuery());
        }

        public virtual DeploymentQuery FetchModel()
        {
            this.fetchModel = true;

            return this;
        }

        public virtual Deployment Single() => this.CreateNativeQuery().SingleOrDefault();

        public virtual IList<Deployment> List() => this.CreateNativeQuery().ToList();

        public virtual Task<IList<Deployment>> ListAsync(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            var query = this.CreateNativeQuery()
                .OrderByDescending(x => x.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<Deployment>> ListAsync(int count)
        {
            IQueryable<Deployment> query = this.CreateNativeQuery()
                .OrderByDescending(x =>x.Created);

            if (count > 0)
                query = query.Take(count);

            return this.Session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<Deployment>> ListAsync()
        {
            return this.Session.QueryMultipleAsync(this.CreateNativeQuery());
        }

        async Task<IList<IDeployment>> IDeploymentQuery.ListAsync()
        {
            var list = await this.ListAsync();
            return list.ToList<IDeployment>();
        }

        async Task<IList<IDeployment>> IDeploymentQuery.ListAsync(int count)
        {
            var list = await this.ListAsync(count);
            return list.ToList<IDeployment>();
        }

        async Task<IDeployment> IDeploymentQuery.SingleAsync()
        {
            return await this.SingleAsync();
        }

        async Task<IList<IDeployment>> IDeploymentQuery.ListAsync(int page, int pageSize)
        {
            var list = await this.ListAsync(page, pageSize);
            return list.ToList<IDeployment>();
        }

        public virtual DeploymentQuery SetCategory(string category)
        {
            this.category = category;

            return this;
        }

        public virtual DeploymentQuery SetUserId(string userId)
        {
            this.userId = userId;

            return this;
        }

        public virtual DeploymentQuery SetId(int id)
        {
            this.id = id;

            return this;
        }

        public virtual DeploymentQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual DeploymentQuery SetPackageId(int packageId)
        {
            this.packageId = packageId;

            return this;
        }

        public virtual DeploymentQuery SetCreatedFrom(DateTime fromDate)
        {
            this.createdFrom = fromDate;

            return this;
        }

        public virtual DeploymentQuery SetCreatedTo(DateTime toDate)
        {
            this.createdTo = toDate;

            return this;
        }

        public virtual int Count() => this.CreateNativeQuery().Count();

        public virtual Task<Deployment> SingleAsync()
        {
            return this.Session.QuerySingleAsync(this.CreateNativeQuery());
        }

        IDeploymentQuery IDeploymentQuery.SetId(int id)
            => this.SetId(id);

        IDeploymentQuery IDeploymentQuery.SetName(string name)
            => this.SetName(name);

        IDeploymentQuery IDeploymentQuery.SetCategory(string category)
            => this.SetCategory(category);

        IDeploymentQuery IDeploymentQuery.SetPackageId(int packageId)
            => this.SetPackageId(packageId);

        IDeploymentQuery IDeploymentQuery.SetUserId(string userId) => this.SetUserId(userId);

        IDeploymentQuery IDeploymentQuery.SetCreatedFrom(DateTime fromDate)
            => this.SetCreatedFrom(fromDate);

        IDeploymentQuery IDeploymentQuery.SetCreatedTo(DateTime toDate)
            => this.SetCreatedTo(toDate);

        IDeploymentQuery IDeploymentQuery.FetchModel() => this.FetchModel();

        Task<int> IDeploymentQuery.CountAsync() => this.CountAsync();

        IDeployment IDeploymentQuery.Single() => this.Single();

        int IDeploymentQuery.Count() => this.Count();

        IList<IDeployment> IDeploymentQuery.List() => this.List().ToList<IDeployment>();
    }
}
