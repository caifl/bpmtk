using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine
{
    [Serializable]
    public class ProcessEngineOptions
    {
        protected readonly ConcurrentDictionary<string, object> properties = new ConcurrentDictionary<string, object>();
        protected readonly Dictionary<string, AssignmentStrategyEntry> assignmentStrategyEntries = new Dictionary<string, AssignmentStrategyEntry>();
        protected readonly List<IProcessEventListener> processEventListeners = new List<IProcessEventListener>();
        protected readonly List<ITaskEventListener> taskEventListeners = new List<ITaskEventListener>();

        /// <summary>
        /// Gets or sets engine name.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets engine properties.
        /// </summary>
        public virtual IReadOnlyDictionary<string, object> Properties
        {
            get => this.properties;
        }

        public virtual object GetProperty(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            object value = null;
            if (this.properties.TryGetValue(name, out value))
                return value;

            return null;
        }

        public virtual ProcessEngineOptions SetProperty(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.properties.AddOrUpdate(name, value, (k, v) => value);

            return this;
        }

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

        public virtual ProcessEngineOptions AddProcessEventListener(IProcessEventListener processEventListener)
        {
            if (processEventListener == null)
                throw new ArgumentNullException(nameof(processEventListener));

            if (!this.processEventListeners.Contains(processEventListener))
                this.processEventListeners.Add(processEventListener);

            return this;
        }

        public virtual ProcessEngineOptions AddTaskEventListener(ITaskEventListener taskEventListener)
        {
            if (taskEventListener == null)
                throw new ArgumentNullException(nameof(taskEventListener));

            if (!this.taskEventListeners.Contains(taskEventListener))
                this.taskEventListeners.Add(taskEventListener);

            return this;
        }

        public virtual ProcessEngineOptions AddTaskAssignmentStrategy(string key, string name,
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
