using System;

namespace Bpmtk.Engine.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
