using System;
using System.Threading;
using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bpmtk.Engine.Scheduler;
using Bpmtk.Engine.Identity;
using Bpmtk.Engine.Infrastructure;

namespace Bpmtk.Engine
{
    public class Context : IContext, IDisposable
    {
        private static AsyncLocal<IContext> current = new AsyncLocal<IContext>();
        private readonly ProcessEngine engine;

        internal Context(ProcessEngine engine, IDbSession dbSession)
        {
            this.engine = engine;
            this.DbSession = dbSession;
        }

        public virtual IDbSession DbSession
        {
            get;
        }

        public static IContext Current => current.Value;

        public virtual ProcessEngine Engine => this.engine;

        IProcessEngine IContext.Engine => this.engine;

        public virtual int UserId
        {
            get;
            protected set;
        }

        public virtual ITaskManager TaskManager
        {
            get => new TaskManager(this);
        }

        public virtual IDeploymentManager DeploymentManager
        {
            get => new DeploymentManager(this);
        }

        public virtual IHistoryManager HistoryManager => new HistoryManager(this);

        public virtual IRuntimeManager RuntimeManager => new RuntimeManager(this);

        public virtual IIdentityManager IdentityManager => new IdentityManager(this);

        public virtual IScheduledJobManager ScheduledJobManager => new ScheduledJobManager(this);

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

        public virtual IContext SetAuthenticatedUser(int userId)
        {
            this.UserId = userId;

            return this;
        }

        public virtual async Task<ProcessInstance> StartProcessByKeyAsync(string processDefintionKey,
            IDictionary<string, object> variables = null)
        {
            var processDefinition = await this.DeploymentManager.FindProcessDefinitionByKeyAsync(processDefintionKey);
            var builder = this.RuntimeManager.CreateProcessInstanceBuilder();
            builder.SetProcessDefinition(processDefinition);
            var procInst = await builder.BuildAsync();

            //var initialNode = null;

            var token = new Token(procInst);
            var executionContext = Runtime.ExecutionContext.Create(this, token);
            await executionContext.StartAsync(null);

            return procInst;
        }

        public virtual Task<ProcessInstance> StartProcessByMessageAsync(string messageName,
            IDictionary<string, object> messageData = null)
        {
            //var eventSubscr = this.eventSubscriptions.FindByName(messageName, "message");
            //if (eventSubscr == null)
            //    throw new RuntimeException($"The message '{messageName}' event handler does not exists.");

            //if (eventSubscr.ProcessDefinition != null
            //    && eventSubscr.ActivityId != null)
            //{
            //    IMessageStartEventHandler handler = new MessageStartEventHandler(this.deploymentManager,
            //        this.instances);

            //    var task = handler.Execute(eventSubscr, messageData);

            //    return task.Result;
            //}
            throw new NotImplementedException();
        }

        public virtual ITransaction BeginTransaction()
            => this.DbSession.BeginTransaction();

        #endregion
    }
}
