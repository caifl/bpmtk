using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine
{
    public interface IDeploymentManager
    {
        /// <summary>
        /// Create deployment query.
        /// </summary>
        IDeploymentQuery CreateDeploymentQuery();

        /// <summary>
        /// Create process definition query.
        /// </summary>
        IProcessDefinitionQuery CreateDefinitionQuery();

        /// <summary>
        /// Find the specified deployment.
        /// </summary>
        IDeployment Find(int deploymentId);

        Task AddIdentityLinksAsync(int processDefinitionId, params IdentityLink[] identityLinks);

        Task<IList<IdentityLink>> GetIdentityLinksAsync(int processDefintionId);

        Task RemoveIdentityLinksAsync(params long[] identityLinkIds);

        IProcessDefinition FindProcessDefinitionById(int processDefinitionId);

        IProcessDefinition FindProcessDefinitionByKey(string processDefinitionKey);

        IProcessDefinition InactivateProcessDefinition(int processDefinitionId, 
            string comment = null);

        IProcessDefinition ActivateProcessDefinition(int processDefinitionId,
            string comment = null);

        Task<IProcessDefinition> InactivateProcessDefinitionAsync(int processDefinitionId,
            string comment = null);

        Task<IProcessDefinition> ActivateProcessDefinitionAsync(int processDefinitionId,
            string comment = null);

        IList<Comment> GetCommentsForProcessDefinition(int processDefinitionId);

        IList<EventSubscription> GetEventSubscriptions(int processDefintionId);

        IList<ScheduledJob> GetScheduledJobs(int processDefintionId);

        BpmnModel GetBpmnModel(int deploymentId);

        Task<Byte[]> GetBpmnModelContentAsync(int deploymentId);

        IDeploymentBuilder CreateDeploymentBuilder();
    }
}
