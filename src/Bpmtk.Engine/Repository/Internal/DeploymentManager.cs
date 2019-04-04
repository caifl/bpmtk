using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Repository.Internal
{
    public class DeploymentManager : IDeploymentManager
    {
        /// <summary>
        /// BPMN Object Model cached by DeploymentId.
        /// </summary>
        private static readonly ConcurrentDictionary<int, BpmnModel> modelCache = new ConcurrentDictionary<int, BpmnModel>();

        private readonly IDeploymentStore deployments;
        private readonly IEventSubscriptionStore eventSubscriptions;
        private readonly IScheduledJobStore scheduledJobStore;

        public DeploymentManager(IDeploymentStore deployments,
            IEventSubscriptionStore eventSubscriptions,
            IScheduledJobStore scheduledJobStore)
        {
            this.deployments = deployments;
            this.eventSubscriptions = eventSubscriptions;
            this.scheduledJobStore = scheduledJobStore;
        }

        public virtual IDeploymentBuilder CreateDeploymentBuilder()
        {
            return new DeploymentBuilder(this.deployments,
                this.eventSubscriptions,
                this.scheduledJobStore);
        }

        public virtual BpmnModel GetBpmnModel(int deploymentId)
        {
            var model = modelCache.GetOrAdd(deploymentId, (id) =>
            {
                var data = deployments.GetBpmnModelData(id);

                return BpmnModel.FromBytes(data);
            });

            return model;
        }
    }
}
