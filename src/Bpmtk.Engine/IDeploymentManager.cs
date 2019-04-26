using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IDeploymentManager
    {
        IQueryable<Deployment> Deployments
        {
            get;
        }

        IQueryable<ProcessDefinition> ProcessDefinitions
        {
            get;
        }

        Task<Deployment> FindAsync(int deploymentId);

        Task<ProcessDefinition> FindProcessDefinitionByIdAsync(int processDefinitionId);

        Task<ProcessDefinition> FindProcessDefinitionByKeyAsync(string processDefinitionKey);

        Task<IList<EventSubscription>> GetEventSubscriptionsAsync(int processDefintionId);

        Task<IList<ScheduledJob>> GetScheduledJobsAsync(int processDefintionId);

        Task<BpmnModel> GetBpmnModelAsync(int deploymentId);

        IDeploymentBuilder CreateDeploymentBuilder();
    }
}
