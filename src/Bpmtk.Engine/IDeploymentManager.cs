using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine
{
    public interface IDeploymentManager
    {
        //IQueryable<Deployment> Deployments
        //{
        //    get;
        //}

        IDeploymentQuery CreateDeploymentQuery();

        IProcessDefinitionQuery CreateDefinitionQuery();

        IQueryable<ProcessDefinition> ProcessDefinitions
        {
            get;
        }

        Task<Deployment> FindAsync(int deploymentId);

        Task AddIdentityLinksAsync(int processDefinitionId, params IdentityLink[] identityLinks);

        Task<IList<IdentityLink>> GetIdentityLinksAsync(int processDefintionId);

        Task RemoveIdentityLinksAsync(params long[] identityLinkIds);

        Task<ProcessDefinition> FindProcessDefinitionByIdAsync(int processDefinitionId);

        Task<ProcessDefinition> FindProcessDefinitionByKeyAsync(string processDefinitionKey);

        Task InactivateProcessDefinitionAsync(int processDefinitionId, 
            string comment = null);

        Task ActivateProcessDefinitionAsync(int processDefinitionId,
            string comment = null);

        Task<IList<Comment>> GetCommentsForProcessDefinitionAsync(int processDefinitionId);

        Task<IList<EventSubscription>> GetEventSubscriptionsAsync(int processDefintionId);

        Task<IList<ScheduledJob>> GetScheduledJobsAsync(int processDefintionId);

        Task<BpmnModel> GetBpmnModelAsync(int deploymentId);

        IDeploymentBuilder CreateDeploymentBuilder();
    }
}
