using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Repository;
using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Stores.Internal
{
    public class DeploymentStore : IDeploymentStore
    {
        private readonly ISession session;

        public DeploymentStore(ISession session)
        {
            this.session = session;
        }

        public virtual Deployment FindDeplymentById(int deploymentId)
        {
            return this.session.Get<Deployment>(deploymentId);
        }

        public virtual byte[] GetBpmnModelData(int deploymentId)
        {
            return this.session.Query<Deployment>().Where(x => x.Id == deploymentId)
                .Select(x => x.Model.Value)
                .SingleOrDefault();
        }

        public virtual Deployment GetDeploymentWithModel(int deploymentId)
        {
            return this.session.Query<Deployment>().Where(x => x.Id == deploymentId).SingleOrDefault();
        }

        public virtual ProcessDefinition GetProcessDefintionById(int id)
            => this.session.Get<ProcessDefinition>(id);

        public virtual ProcessDefinition GetProcessDefintionByKey(string key)
        {
            return this.session.Query<ProcessDefinition>()
                .Where(x => x.Key == key)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault();
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

        public void Add(Deployment deployment)
        {
            this.session.Save(deployment);
        }

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
    }
}
