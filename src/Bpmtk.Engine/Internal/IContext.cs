using System;

namespace Bpmtk.Engine
{
    public interface IContext : IDisposable
    {
        IProcessEngine Engine
        {
            get;
        }

        TService GetService<TService>();

        int UserId
        {
            get;
        }

        IContext SetAuthenticatedUser(int userId);
    }
}
