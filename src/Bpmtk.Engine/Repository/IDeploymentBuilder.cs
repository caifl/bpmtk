using System;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Repository
{
    public interface IDeploymentBuilder
    {
        IDeploymentBuilder SetTanentId(string tanentId);

        IDeploymentBuilder SetName(string name);

        IDeploymentBuilder SetMemo(string memo);

        IDeploymentBuilder SetCategory(string category);

        IDeploymentBuilder SetBpmnModel(byte[] modelData);

        /// <summary>
        /// Set source package identifier.
        /// </summary>
        IDeploymentBuilder SetPackageId(int packageId);

        IDeployment Build();

        Task<IDeployment> BuildAsync();
    }
}
