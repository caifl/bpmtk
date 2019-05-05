using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Identity;
using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Tasks;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine
{
    public class ProcessEngineBuilder : IProcessEngineBuilder
    {
        protected ProcessEngineOptions options = new ProcessEngineOptions();
        protected IContextFactory contextFactory;
        protected ILoggerFactory loggerFactory;
        protected Action<ProcessEngineOptions> optionsAction;

        public virtual IProcessEngine Build()
        {
            var options = new ProcessEngineOptions();

            //Configure engine.
            if (this.optionsAction != null)
                this.optionsAction.Invoke(options);

            return new ProcessEngine(this.contextFactory, this.loggerFactory, options);
        }

        public virtual IContextFactory ContextFactory
        {
            get => this.contextFactory;
        }

        public virtual ILoggerFactory LoggerFactory
        {
            get => this.loggerFactory;
        }

        public virtual IProcessEngineBuilder SetContextFactory(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;

            return this;
        }

        public virtual IProcessEngineBuilder SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;

            return this;
        }

        public virtual IProcessEngineBuilder AddProcessEventListener(IProcessEventListener processEventListener)
        {
            if (processEventListener == null)
                throw new ArgumentNullException(nameof(processEventListener));

            this.options.AddProcessEventListener(processEventListener);

            return this;
        }

        public virtual IProcessEngineBuilder AddTaskEventListener(ITaskEventListener taskEventListener)
        {
            if (taskEventListener == null)
                throw new ArgumentNullException(nameof(taskEventListener));

            this.options.AddTaskEventListener(taskEventListener);

            return this;
        }

        public virtual IProcessEngineBuilder AddTaskAssignmentStrategy(string key, string name, 
            IAssignmentStrategy assignmentStrategy)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (assignmentStrategy == null)
                throw new ArgumentNullException(nameof(assignmentStrategy));

            this.options.AddTaskAssignmentStrategy(key, name, assignmentStrategy);

            return this;
        }

        //public virtual IProcessEngineBuilder DisableActivityRecorder()
        //{
        //    this.IsActivityRecorderDisabled = true;

        //    return this;
        //}

        public virtual IProcessEngineBuilder Configure(Action<ProcessEngineOptions> optionsAction)
        {
            this.optionsAction = optionsAction;

            return this;
        }
    }
}
