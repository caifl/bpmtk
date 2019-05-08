using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Repository
{
    public class PackageManager : IPackageManager
    {
        protected Context context;

        public PackageManager(Context context)
        {
            this.context = context;
        }

        public virtual Task CreateAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public IPackageQuery CreateQuery()
        {
            throw new NotImplementedException();
        }

        public Task<Package> FindAsync(int packageId)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetBpmnModelContentAsync(int packageId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int packageId)
        {
            throw new NotImplementedException();
        }
    }
}
