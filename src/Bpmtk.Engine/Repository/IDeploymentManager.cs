using Bpmtk.Engine.Bpmn2;

namespace Bpmtk.Engine.Repository
{
    public interface IDeploymentManager
    {
        BpmnModel GetBpmnModel(int deploymentId);

        IDeploymentBuilder CreateDeploymentBuilder();
    }
}
