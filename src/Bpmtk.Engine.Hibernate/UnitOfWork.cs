using System;
using System.Collections.Concurrent;
using NHibernate;
using Bpmtk.Infrastructure;

namespace Bpmtk.Engine
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly ConcurrentDictionary<Type, IRepository> repositories 
            = new ConcurrentDictionary<Type, IRepository>();

        public UnitOfWork(ISession session)
        {
            this.session = session;
        }

        public virtual void Commit()
        {
            session.Flush();
        }

        public virtual void Dispose()
        {
            this.session.Dispose();
        }

        protected virtual IRepository CreateInstance(Type type)
        {
            return null;
        }

        public virtual TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            var type = typeof(TRepository);
            return (TRepository)this.repositories.GetOrAdd(type, new Func<Type, IRepository>(this.CreateInstance));
        }
    }
}
