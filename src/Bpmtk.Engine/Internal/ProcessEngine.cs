using System;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine.Internal
{
    public class ProcessEngine : IProcessEngine, IDisposable
    {
        private static ProcessEngine instance = null;
        protected readonly static object syncRoot = new object();

        private readonly ProcessEngineOptions options;
        protected readonly IContextFactory contextFactory;
        protected readonly ILoggerFactory loggerFactory;

        public static ProcessEngine GetInstance()
        {
            if (instance == null)
                throw new EngineException("The process engine was not initialized.");

            lock (syncRoot)
            {
                return instance;
            }
        }

        public ProcessEngine(IContextFactory contextFactory, 
            ILoggerFactory loggerFactory,
            ProcessEngineOptions options)
        {
            if (contextFactory == null)
                throw new ArgumentNullException(nameof(contextFactory));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            lock (syncRoot)
                instance = this;

            this.contextFactory = contextFactory;
            this.loggerFactory = loggerFactory;
            this.options = options;

            //init
            //this.ProcessEventListener = new CompositeProcessEventListener(builder.ProcessEventListeners);
            //this.TaskEventListener = new CompositeTaskEventListener(builder.TaskEventListeners);

            //initialize process-engine props.
            // ...
        }

        public virtual bool TryGetValue(string name, out object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.options.Properties.TryGetValue(name, out value);
        }

        public virtual object GetValue(string name, object defaultValue = null)
        {
            object value = defaultValue;

            this.TryGetValue(name, out value);
            return value;
        }

        public virtual TValue GetValue<TValue>(string name, TValue defaultValue = default(TValue))
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var value = this.GetValue(name);

            if (value != null)
                return (TValue)value; //type cast needed..

            return defaultValue;
        }

        IProcessEngine IProcessEngine.SetValue(string name, object value) => this.SetValue(name, value);

        public virtual ProcessEngine SetValue(string name, object value)
        {
            this.options.SetProperty(name, value);

            return this;
        }

        public virtual ProcessEngineOptions Options => this.options;

        public virtual IContext CreateContext()
            => contextFactory.Create(this);

        public virtual ILoggerFactory LoggerFactory => this.loggerFactory;

        //public virtual IProcessEventListener ProcessEventListener
        //{
        //    get;
        //}

        //public virtual ITaskEventListener TaskEventListener
        //{
        //    get;
        //}

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

        //public virtual bool IsActivityRecorderDisabled => this.builder.IsActivityRecorderDisabled;

        

        #endregion
    }
}
