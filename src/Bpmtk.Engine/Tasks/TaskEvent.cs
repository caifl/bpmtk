using System;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Tasks
{
    public abstract class TaskEvent : ITaskEvent
    {
        public TaskEvent(IContext context, TaskInstance task)
        {
            this.Context = context;
            this.Task = task;
        }

        public virtual string Name
        {
            get;
        }

        public virtual IContext Context
        {
            get;
        }

        public virtual ITaskInstance Task
        {
            get;
        }

        public const string TaskCompletedEvent = "completed";
    }

    public class TaskCompletedEvent : TaskEvent
    {
        public TaskCompletedEvent(IContext context, TaskInstance task) : base(context, task)
        {
        }
    }
}
