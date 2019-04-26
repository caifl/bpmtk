using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IDeploymentBuilder
    {
        IDeploymentBuilder SetName(string name);

        IDeploymentBuilder SetMemo(string memo);

        IDeploymentBuilder SetCategory(string category);

        IDeploymentBuilder SetBpmnModel(byte[] modelData);

        IDeploymentBuilder SetPackage(Package package);

        Task<Deployment> BuildAsync();
    }
}
