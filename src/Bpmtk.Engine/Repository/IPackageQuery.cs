using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Repository
{
    public interface IPackageQuery
    {
        IPackageQuery SetId(int id);

        IPackageQuery SetName(string name);

        IPackageQuery SetCategory(string category);

        IPackageQuery SetOwner(string userId);

        IPackageQuery SetCreatedFrom(DateTime fromDate);

        IPackageQuery SetCreatedTo(DateTime toDate);

        IPackageQuery FetchModel();

        IPackage Single();

        int Count();

        IList<IPackage> List();

        Task<IPackage> SingleAsync();

        Task<int> CountAsync();

        Task<IList<IPackage>> ListAsync(int page, int pageSize);

        Task<IList<IPackage>> ListAsync(int count);

        Task<IList<IPackage>> ListAsync();
    }
}
