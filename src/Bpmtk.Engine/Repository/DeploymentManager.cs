using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Repository
{
    public class DeploymentManager : IDeploymentManager
    {
        /// <summary>
        /// BPMN Object Model cached by DeploymentId.
        /// </summary>
        private static readonly ConcurrentDictionary<int, BpmnModel> modelCache = new ConcurrentDictionary<int, BpmnModel>();

        private readonly IDbSession db;
        private readonly Context context;

        public DeploymentManager(Context context)
        {
            this.context = context;
            this.db = context.DbSession;
        }

        public virtual IQueryable<Deployment> Deployments => this.db.Deployments;

        public virtual IQueryable<ProcessDefinition> ProcessDefinitions => this.db.ProcessDefinitions;

        public virtual IDeploymentBuilder CreateDeploymentBuilder()
        {
            return new DeploymentBuilder(context, this);
        }

        public virtual Task<Deployment> FindAsync(int deploymentId)
            => this.db.FindAsync<Deployment>(deploymentId);

        public virtual Task<ProcessDefinition> FindProcessDefinitionByIdAsync(int processDefinitionId)
            => this.db.FindAsync<ProcessDefinition>(processDefinitionId);

        public virtual Task<ProcessDefinition> FindProcessDefinitionByKeyAsync(string processDefinitionKey)
        {
            var query = this.db.ProcessDefinitions
                .Where(x => x.Key == processDefinitionKey)
                .OrderByDescending(x => x.Version)
                .Take(1);

            return this.db.QuerySingleAsync(query);
        }

        public virtual Task<IList<EventSubscription>> GetEventSubscriptionsAsync(int processDefintionId)
        {
            var query = this.db.EventSubscriptions
                .Where(x => x.ProcessDefinition.Id == processDefintionId);

            return this.db.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ScheduledJob>> GetScheduledJobsAsync(int processDefintionId)
        {
            var query = this.db.ScheduledJobs
                .Where(x => x.ProcessDefinition.Id == processDefintionId);

            return this.db.QueryMultipleAsync(query);
        }

        public virtual async Task<BpmnModel> GetBpmnModelAsync(int deploymentId)
        {
            BpmnModel model = null;
            if (!modelCache.ContainsKey(deploymentId))
            {
                var query = this.db.Deployments.Where(x => x.Id == deploymentId)
                    .Select(x => x.Model.Value);

                var bytes = await db.QuerySingleAsync(query);
                model = BpmnModel.FromBytes(bytes);
            }

            return modelCache.GetOrAdd(deploymentId, (id) => model);
        }

        public virtual async Task AddIdentityLinksAsync(int processDefinitionId, params IdentityLink[] identityLinks)
        {
            if(identityLinks != null && identityLinks.Length > 0)
            {
                //Check if identity-link already exists.
                var procDef = await this.FindProcessDefinitionByIdAsync(processDefinitionId);

                var date = Utils.Clock.Now;
                foreach (var item in identityLinks)
                {
                    item.Created = date;
                    item.ProcessDefinition = procDef;
                    //procDef.IdentityLinks.Add(item);
                }

                await this.db.SaveRangeAsync(identityLinks);
                await this.db.FlushAsync();
            }
        }

        public virtual async Task<IList<IdentityLink>> GetIdentityLinksAsync(int processDefintionId)
        {
            var query = this.db.IdentityLinks.Where(x => x.ProcessDefinition.Id == processDefintionId);

            return await this.db.QueryMultipleAsync(query);
        }

        public virtual async Task RemoveIdentityLinksAsync(params long[] identityLinkIds)
        {
            if(identityLinkIds != null && identityLinkIds.Length > 0)
            {
                var query = this.db.IdentityLinks.Where(x => identityLinkIds.Contains(x.Id));
                var items = await this.db.QueryMultipleAsync(query);

                await this.db.RemoveRangeAsync(items);

                await this.db.FlushAsync();
            }
        }
    }
}
