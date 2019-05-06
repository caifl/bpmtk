using System;
using System.Threading;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Scheduler;
using Bpmtk.Engine.Identity;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.History;
using Bpmtk.Engine.Events;

namespace Bpmtk.Engine
{
    public class Context : IContext, IDisposable
    {
        private static AsyncLocal<IContext> current = new AsyncLocal<IContext>();
        private readonly IProcessEngine engine;

        public Context(IProcessEngine engine, IDbSession dbSession)
        {
            this.engine = engine;
            this.DbSession = dbSession;
        }

        public virtual IDbSession DbSession
        {
            get;
        }

        public static IContext Current
        {
            get
            {
                var context = current.Value;
                if(context != null)
                    return context;

                throw new ContextNotBindException("Current context was not binded.");
            }
        }

        public virtual IProcessEngine Engine => this.engine;

        public virtual string UserId
        {
            get;
            protected set;
        }

        public virtual TaskManager TaskManager => new TaskManager(this);

        public virtual DeploymentManager DeploymentManager => new DeploymentManager(this);

        public virtual HistoryManager HistoryManager => new HistoryManager(this);

        public virtual RuntimeManager RuntimeManager => new RuntimeManager(this);

        public virtual IdentityManager IdentityManager => new IdentityManager(this);

        public virtual ScheduledJobManager ScheduledJobManager => new ScheduledJobManager(this);

        ITaskManager IContext.TaskManager => this.TaskManager;

        IHistoryManager IContext.HistoryManager => this.HistoryManager;

        IDeploymentManager IContext.DeploymentManager => this.DeploymentManager;

        IRuntimeManager IContext.RuntimeManager => this.RuntimeManager;

        IIdentityManager IContext.IdentityManager => this.IdentityManager;

        IScheduledJobManager IContext.ScheduledJobManager => this.ScheduledJobManager;

        //public virtual IProcessEventListener ProcessEventListener
        //{
        //    get
        //    {
        //        var options = this.engine.Options;

        //        var eventListener = new CompositeProcessEventListener(options.ProcessEventListeners);
        //        return eventListener;
        //    }
        //}

        //public virtual ITaskEventListener TaskEventListener
        //{
        //    get
        //    {
        //        var options = this.engine.Options;

        //        var eventListener = new CompositeTaskEventListener(options.TaskEventListeners);
        //        return eventListener;
        //    }
        //}

        public static void SetCurrent(IContext context)
        {
            current.Value = context;
        }

        //public virtual TService GetService<TService>() => 
        //    this.scope.ServiceProvider.GetRequiredService<TService>();

        #region IDisposable Support

        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //if(this.scope != null)
                    //    this.scope.Dispose();
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

        public virtual IContext SetAuthenticatedUser(string userId)
        {
            this.UserId = userId;

            return this;
        }

        public virtual ITransaction BeginTransaction()
            => this.DbSession.BeginTransaction();

        #endregion
    }
}
