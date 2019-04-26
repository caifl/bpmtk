using System;
using Microsoft.EntityFrameworkCore.Storage;
using Bpmtk.Engine.Infrastructure;

namespace Bpmtk.Engine
{
    public class ContextTransaction : ITransaction, IDisposable
    {
        private readonly IDbContextTransaction transaction;

        public ContextTransaction(IDbContextTransaction transaction)
        {
            this.transaction = transaction;
        }

        public virtual void Commit()
        {
            this.transaction.Commit();
        }

        public virtual void Rollback()
        {
            this.transaction.Rollback();
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ContextTransaction()
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
