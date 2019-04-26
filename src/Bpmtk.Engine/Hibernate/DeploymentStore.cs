using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;
using NHibernate;
using NHibernate.Linq;

namespace Bpmtk.Engine.Hibernate
{
    public class DeploymentStore : IDeploymentStore
    {
        private readonly ISession session;

        public virtual IQueryable<Deployment> Deployments => this.session.Query<Deployment>();

        public virtual IQueryable<ProcessDefinition> ProcessDefinitions => this.session.Query<ProcessDefinition>();

        public DeploymentStore(ISession session)
        {
            this.session = session;
        }

        public virtual int GetProcessDefintionNextVersion(string key)
        {
            var value = this.session.Query<ProcessDefinition>()
                .Where(x => x.Key == key)
                .Max(x => (int?)x.Version);

            return value.HasValue ? value.Value + 1 : 1;
        }

        //public virtual async Task<IDictionary<string, int>> GetProcessDefinitionVersionsAsync(params string[] processDefinitionKeys)
        //{
        //    var keys = processDefinitionKeys;
        //    var results = await this.session.Query<ProcessDefinition>()
        //        .Where(x => keys.Contains(x.Key))
        //        .GroupBy(x => x.Key)
        //        .Select(x => new
        //        {
        //            Key = x.Key,
        //            Version = x.Max(y => y.Version)
        //        }).ToListAsync();

        //    Dictionary<string, int> map = new Dictionary<string, int>();
        //    foreach(var item in results)
        //    {
        //        map.Add(item.Key, item.Version);
        //    }

        //    return map;
        //}

        public virtual async Task<IDictionary<string, ProcessDefinition>> 
            GetProcessDefinitionLatestVersionsAsync(params string[] processDefinitionKeys)
        {
            var keys = processDefinitionKeys;
            var results = await this.session.Query<ProcessDefinition>()
                .Where(x => keys.Contains(x.Key))
                .GroupBy(x => x.Key)
                .Select(x => new
                {
                    Key = x.Key,
                    Version = x.OrderByDescending(y => y.Version).FirstOrDefault()
                }).ToListAsync();

            Dictionary<string, ProcessDefinition> map = new Dictionary<string, ProcessDefinition>();
            foreach (var item in results)
            {
                map.Add(item.Key, item.Version);
            }

            return map;
        }

        public virtual Task CreateAsync(Deployment deployment)
        {
            return this.session.SaveAsync(deployment);
        }

        public virtual Task<Deployment> FindDeplymentByIdAsync(int deploymentId)
        {
            return this.session.GetAsync<Deployment>(deploymentId);
        }

        public virtual Task<Deployment> FindDeplymentByIdAsync(int deploymentId, bool includeModel)
        {
            return this.session.GetAsync<Deployment>(deploymentId);
        }

        public virtual Task<byte[]> GetBpmnModelAsync(int deploymentId)
        {
            return this.Deployments.Where(x => x.Id == deploymentId)
                .Select(x => x.Model.Value)
                .SingleOrDefaultAsync();
        }

        public virtual Task<ProcessDefinition> FindProcessDefintionByKeyAsync(string processDefinitionKey)
        {
            return this.ProcessDefinitions
                .Where(x => x.Key == processDefinitionKey)
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync();
        }

        public virtual Task<ProcessDefinition> FindProcessDefintionByIdAsync(int processDefinitionId)
        {
            return this.session.GetAsync<ProcessDefinition>(processDefinitionId);
        }
    }
}
