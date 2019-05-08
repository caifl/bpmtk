using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine
{
    public interface IPackageManager
    {
        IPackageQuery CreateQuery();

        Task CreateAsync(Package package);

        Task<Package> FindAsync(int packageId);

        Task RemoveAsync(int packageId);

        Task<byte[]> GetBpmnModelContentAsync(int packageId);
    }
}
