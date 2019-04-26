using System;
using System.Collections.Concurrent;
using NHibernate;
using Bpmtk.Infrastructure;

namespace Bpmtk.Engine
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ISession session;
        private readonly ITransaction transaction;
        //private readonly ConcurrentDictionary<Type, IRepository> repositories 
        //    = new ConcurrentDictionary<Type, IRepository>();

        public UnitOfWork(ISession session)
        {
            this.session = session;
            this.transaction = session.BeginTransaction();
        }

        public virtual bool WasCommitted
        {
            get => this.transaction.WasCommitted;
        }

        public virtual void Commit()
        {
            if (this.WasCommitted)
                throw new Exception("The transaction has been committed already.");

            session.Flush();
            transaction.Commit();
        }

        public virtual TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            return Context.Current.GetService<TRepository>();
            //var type = typeof(TRepository);
            //return (TRepository)this.repositories.GetOrAdd(type, new Func<Type, IRepository>(this.CreateInstance));
        }

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.transaction.Dispose();
                    this.session.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
