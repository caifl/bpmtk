using Microsoft.Extensions.Logging;
using System;

namespace Bpmtk.Engine.Internal
{
    public class ProcessEngine : IProcessEngine, IDisposable
    {
        private readonly ProcessEngineBuilder builder;
        private static ProcessEngine instance = null;

        public static ProcessEngine GetInstance() => instance;

        public ProcessEngine(ProcessEngineBuilder builder)
        {
            instance = this;
            this.builder = builder;
        }

        public virtual IContext CreateContext()
        {
            var sessionFactory = builder.DbSessionFactory;
            var session = sessionFactory.Create();
            return new Context(this, session);
        }

        public virtual ILoggerFactory LoggerFactory => this.builder.LoggerFactory;

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcessEngine()
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
