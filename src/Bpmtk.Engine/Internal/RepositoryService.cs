using System;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Internal
{
    public class RepositoryService : IRepositoryService
    {
        protected readonly IDeploymentStore deployments;
        protected readonly IEventSubscriptionStore eventSubscriptions;
        protected readonly IDeploymentManager deploymentManager;

        public RepositoryService(IDeploymentStore deployments,
            IEventSubscriptionStore eventSubscriptions,
            IDeploymentManager deploymentManager)
        {
            this.deployments = deployments;
            this.eventSubscriptions = eventSubscriptions;
            this.deploymentManager = deploymentManager;
        }

        public virtual IDeploymentBuilder CreateDeploymentBuilder()
        {
            return this.deploymentManager.CreateDeploymentBuilder();
        }

        public virtual byte[] GetBpmnModelData(int deploymentId)
        {
            //var map = this.deployments.GetProcessDefinitionVersions("1", "2", "3");

            var data = this.deployments.GetBpmnModelData(deploymentId);

            return data;
        }
    }
}
