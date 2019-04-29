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
        protected readonly Dictionary<string, AssignmentStrategyEntry> assignmentStrategyEntries 
            = new Dictionary<string, AssignmentStrategyEntry>();

        protected readonly List<IProcessEventListener> processEventListeners = new List<IProcessEventListener>();
        protected readonly List<ITaskEventListener> taskEventListeners = new List<ITaskEventListener>();

        public virtual IReadOnlyList<ITaskEventListener> TaskEventListeners
        {
            get => this.taskEventListeners.AsReadOnly();
        }

        public virtual IReadOnlyList<IProcessEventListener> ProcessEventListeners
        {
            get => this.processEventListeners.AsReadOnly();
        }

        public virtual IReadOnlyDictionary<string, AssignmentStrategyEntry> AssignmentStrategyEntries
        {
            get => this.assignmentStrategyEntries;
        }

        public virtual IProcessEngine Build()
        {
            return new ProcessEngine(this);
        }

        protected IContextFactory contextFactory;
        protected ILoggerFactory loggerFactory;

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

            if (!this.processEventListeners.Contains(processEventListener))
                this.processEventListeners.Add(processEventListener);

            return this;
        }

        public virtual IProcessEngineBuilder AddTaskEventListener(ITaskEventListener taskEventListener)
        {
            if (taskEventListener == null)
                throw new ArgumentNullException(nameof(taskEventListener));

            if (!this.taskEventListeners.Contains(taskEventListener))
                this.taskEventListeners.Add(taskEventListener);

            return this;
        }

        public virtual IProcessEngineBuilder AddTaskAssignmentStrategy(string key, string name, 
            IAssignmentStrategy assignmentStrategy)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (assignmentStrategy == null)
                throw new ArgumentNullException(nameof(assignmentStrategy));

            var entry = new AssignmentStrategyEntry(key, name, assignmentStrategy);
            this.assignmentStrategyEntries.Add(key, entry);

            return this;
        }
    }
}
