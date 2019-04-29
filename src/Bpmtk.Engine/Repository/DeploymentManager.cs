using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Repository
{
    public class DeploymentManager : IDeploymentManager
    {
        /// <summary>
        /// BPMN Object Model cached by DeploymentId.
        /// </summary>
        private static readonly ConcurrentDictionary<int, BpmnModel> modelCache = new ConcurrentDictionary<int, BpmnModel>();

        private readonly IDbSession session;
        private readonly Context context;

        public DeploymentManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;
        }

        public virtual IQueryable<Deployment> Deployments => this.session.Deployments;

        public virtual IQueryable<ProcessDefinition> ProcessDefinitions => this.session.ProcessDefinitions;

        public virtual IDeploymentBuilder CreateDeploymentBuilder()
        {
            return new DeploymentBuilder(context, this);
        }

        public virtual Task<Deployment> FindAsync(int deploymentId)
            => this.session.FindAsync<Deployment>(deploymentId);

        public virtual Task<ProcessDefinition> FindProcessDefinitionByIdAsync(int processDefinitionId)
            => this.session.FindAsync<ProcessDefinition>(processDefinitionId);

        public virtual Task<ProcessDefinition> FindProcessDefinitionByKeyAsync(string processDefinitionKey)
        {
            var query = this.session.ProcessDefinitions
                .Where(x => x.Key == processDefinitionKey)
                .OrderByDescending(x => x.Version)
                .Take(1);

            return this.session.QuerySingleAsync(query);
        }

        public virtual Task<IList<EventSubscription>> GetEventSubscriptionsAsync(int processDefintionId)
        {
            var query = this.session.EventSubscriptions
                .Where(x => x.ProcessDefinition.Id == processDefintionId);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<ScheduledJob>> GetScheduledJobsAsync(int processDefintionId)
        {
            var query = this.session.ScheduledJobs
                .Where(x => x.ProcessDefinition.Id == processDefintionId);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<BpmnModel> GetBpmnModelAsync(int deploymentId)
        {
            BpmnModel model = null;
            if (!modelCache.ContainsKey(deploymentId))
            {
                var query = this.session.Deployments.Where(x => x.Id == deploymentId)
                    .Select(x => x.Model.Value);

                var bytes = await session.QuerySingleAsync(query);
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

                await this.session.SaveRangeAsync(identityLinks);
                await this.session.FlushAsync();
            }
        }

        public virtual async Task<IList<IdentityLink>> GetIdentityLinksAsync(int processDefintionId)
        {
            var query = this.session.IdentityLinks.Where(x => x.ProcessDefinition.Id == processDefintionId);

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task RemoveIdentityLinksAsync(params long[] identityLinkIds)
        {
            if(identityLinkIds != null && identityLinkIds.Length > 0)
            {
                var query = this.session.IdentityLinks.Where(x => identityLinkIds.Contains(x.Id));
                var items = await this.session.QueryMultipleAsync(query);

                await this.session.RemoveRangeAsync(items);

                await this.session.FlushAsync();
            }
        }

        public virtual IDeploymentQuery CreateQuery() => new DeploymentQuery(this.session);

        public virtual async Task InactivateProcessDefinitionAsync(int processDefinitionId, 
            string comment = null)
        {
            var procDef = await this.FindProcessDefinitionByIdAsync(processDefinitionId);
            if (procDef != null)
            {
                procDef.State = ProcessDefinitionState.Inactive;

                var item = new Comment();
                item.ProcessDefinition = procDef;
                item.Body = comment;
                item.Created = Clock.Now;
                item.UserId = this.context.UserId;

                await this.session.SaveAsync(item);

                await this.session.FlushAsync();
            }
        }

        public virtual async Task ActivateProcessDefinitionAsync(int processDefinitionId, string comment = null)
        {
            var procDef = await this.FindProcessDefinitionByIdAsync(processDefinitionId);
            if (procDef != null)
            {
                procDef.State = ProcessDefinitionState.Active;

                var item = new Comment();
                item.ProcessDefinition = procDef;
                item.Body = comment;
                item.Created = Clock.Now;
                item.UserId = this.context.UserId;

                await this.session.SaveAsync(item);

                await this.session.FlushAsync();
            }
        }

        public virtual Task<IList<Comment>> GetCommentsForProcessDefinitionAsync(int processDefinitionId)
        {
            var query = this.session.Query<Comment>();
            query = this.session.Fetch(query, x => x.User)
                .Where(x => x.ProcessDefinition.Id == processDefinitionId)
                .OrderByDescending(x => x.Created);

            return this.session.QueryMultipleAsync(query);
        }
    }
}
