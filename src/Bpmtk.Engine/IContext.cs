
using System;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine
{
    public interface IContext : IDisposable
    {
        IProcessEngine Engine
        {
            get;
        }

        ITaskManager TaskManager
        {
            get;
        }

        IHistoryManager HistoryManager
        {
            get;
        }

        IDeploymentManager DeploymentManager
        {
            get;
        }

        IRuntimeManager RuntimeManager
        {
            get;
        }

        IIdentityManager IdentityManager
        {
            get;
        }

        IScheduledJobManager ScheduledJobManager
        {
            get;
        }

        IDbSession DbSession
        {
            get;
        }

        //TService GetService<TService>();

        int UserId
        {
            get;
        }

        IContext SetAuthenticatedUser(int userId);

        ITransaction BeginTransaction();
    }
}
