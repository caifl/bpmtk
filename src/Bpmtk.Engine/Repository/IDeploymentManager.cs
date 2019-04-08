using Bpmtk.Engine.Bpmn2;

namespace Bpmtk.Engine.Repository
{
    public interface IDeploymentManager
    {
        ProcessDefinition FindLatestProcessDefinitionByKey(string processDefinitionKey);

        BpmnModel GetBpmnModel(int deploymentId);

        IDeploymentBuilder CreateDeploymentBuilder();
    }
}
