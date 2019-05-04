using Bpmtk.Engine.Events;
using Bpmtk.Engine.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Bpmtk.Engine.Internal
{
    public class ProcessEngine : IProcessEngine, IDisposable
    {
        private static ProcessEngine instance = null;
        protected readonly static object syncRoot = new object();

        private readonly ProcessEngineBuilder builder;

        //process-engine properties.
        private readonly ConcurrentDictionary<string, object> props = new ConcurrentDictionary<string, object>();

        public static ProcessEngine GetInstance()
        {
            if (instance == null)
                throw new EngineException("The process engine was not initialized.");

            lock (syncRoot)
            {
                return instance;
            }
        }

        public ProcessEngine(ProcessEngineBuilder builder)
        {
            lock(syncRoot)
                instance = this;

            this.builder = builder;

            //init
            this.ProcessEventListener = new CompositeProcessEventListener(builder.ProcessEventListeners);
            this.TaskEventListener = new CompositeTaskEventListener(builder.TaskEventListeners);

            //initialize process-engine props.
            // ...
        }

        public virtual bool TryGetValue(string name, out object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.props.TryGetValue(name, out value);
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
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.props.AddOrUpdate(name, value, (k, v) => value);

            return this;
        }

        public virtual IContext CreateContext()
        {
            var contextFactory = builder.ContextFactory;
            return contextFactory.Create(this);
        }

        public virtual ILoggerFactory LoggerFactory => this.builder.LoggerFactory;

        public virtual IProcessEventListener ProcessEventListener
        {
            get;
        }

        public virtual ITaskEventListener TaskEventListener
        {
            get;
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

        public virtual IAssignmentStrategy GetTaskAssignmentStrategy(string key)
        {
            AssignmentStrategyEntry item = null;

            if (this.builder.AssignmentStrategyEntries.TryGetValue(key, out item))
                return item.AssignmentStrategy;

            return null;
        }

        public virtual IReadOnlyList<AssignmentStrategyEntry> GetTaskAssignmentStrategyEntries()
        {
            var list = new List<AssignmentStrategyEntry>(this.builder.AssignmentStrategyEntries.Values);
            return list.AsReadOnly();
        }

        #endregion
    }
}
