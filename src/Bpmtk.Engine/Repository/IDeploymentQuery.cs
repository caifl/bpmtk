using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Repository
{
    public interface IDeploymentQuery
    {
        IDeploymentQuery SetId(int id);

        IDeploymentQuery SetName(string name);

        IDeploymentQuery SetCategory(string category);

        IDeploymentQuery SetPackageId(int packageId);

        IDeploymentQuery SetUser(int userId);

        IDeploymentQuery SetCreatedFrom(DateTime fromDate);

        IDeploymentQuery SetCreatedTo(DateTime toDate);

        IDeploymentQuery FetchUser();

        IDeploymentQuery FetchModel();

        Task<IDeployment> SingleAsync();

        Task<int> CountAsync();

        Task<IList<IDeployment>> ListAsync(int page, int pageSize);

        Task<IList<IDeployment>> ListAsync(int count);

        Task<IList<IDeployment>> ListAsync();
    }
}
