using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine
{
    public interface IRepositoryService
    {
        IDeploymentBuilder CreateDeploymentBuilder();

        byte[] GetBpmnModelData(int deploymentId);
    }
}
