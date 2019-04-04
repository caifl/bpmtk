using System;

namespace Bpmtk.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        //TRepository GetRepository<TRepository>()
        //    where TRepository : IRepository;

        void Commit();
    }
}
