using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

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

        Task<Deployment> SingleAsync();

        Task<int> CountAsync();

        Task<IList<Deployment>> ListAsync(int page = 1, int pageSize = 20);

        Task<IList<Deployment>> ListAsync();
    }
}
