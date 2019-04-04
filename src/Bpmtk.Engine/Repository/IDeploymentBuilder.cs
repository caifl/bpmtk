using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Repository
{
    public interface IDeploymentBuilder
    {
        IDeploymentBuilder SetName(string name);

        IDeploymentBuilder SetMemo(string memo);

        IDeploymentBuilder SetCategory(string category);

        IDeploymentBuilder SetBpmnModel(byte[] modelData);

        IDeploymentBuilder SetPackage(Package package);

        IDeployment Build();

        Task<IDeployment> BuildAsync();
    }
}
