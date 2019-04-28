using System;

namespace Bpmtk.Engine.Storage
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
