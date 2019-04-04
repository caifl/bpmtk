using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Bpmtk.Engine.Internal;

namespace Bpmtk.Engine
{
    public class Context : IContext, IDisposable
    {
        private static AsyncLocal<IContext> current = new AsyncLocal<IContext>();
        private readonly ProcessEngine engine;
        private readonly IServiceScope scope;
        private readonly IServiceProvider services;

        internal Context(ProcessEngine engine, IServiceScope scope)
        {
            this.engine = engine;
            this.scope = scope;
            this.services = scope.ServiceProvider;
        }

        internal Context(ProcessEngine engine, IServiceProvider services)
        {
            this.engine = engine;
            this.services = services;
        }

        public static IContext Current => current.Value;

        public virtual ProcessEngine Engine => this.engine;

        IProcessEngine IContext.Engine => this.engine;

        public virtual int UserId
        {
            get;
            protected set;
        }

        public static void SetCurrent(IContext context)
        {
            current.Value = context;
        }

        public virtual TService GetService<TService>() => 
            this.scope.ServiceProvider.GetRequiredService<TService>();

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if(this.scope != null)
                        this.scope.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Context()
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

        public virtual IContext SetAuthenticatedUser(int userId)
        {
            this.UserId = userId;

            return this;
        }
        #endregion
    }
}
