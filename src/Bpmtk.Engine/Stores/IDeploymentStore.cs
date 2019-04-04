using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.Stores
{
    public interface IDeploymentStore
    {
        void Add(Deployment deployment);

        Deployment FindDeplymentById(int deploymentId);

        Deployment GetDeploymentWithModel(int deploymentId);

        byte[] GetBpmnModelData(int deploymentId);

        /// <summary>
        /// Get the latest process-definition version by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ProcessDefinition GetProcessDefintionByKey(string key);

        ProcessDefinition GetProcessDefintionById(int id);

        //int GetProcessDefintionNextVersion(string key);

        //Task<IDictionary<string, int>> GetProcessDefinitionVersionsAsync(params string[] processDefinitionKeys);

        Task<IDictionary<string, ProcessDefinition>> GetProcessDefinitionLatestVersionsAsync(params string[] processDefinitionKeys);
    }
}
