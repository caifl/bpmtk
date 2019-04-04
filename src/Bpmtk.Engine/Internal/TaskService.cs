using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Internal
{
    public class TaskService : ITaskService
    {
        private readonly ITaskStore tasks;

        public TaskService(ITaskStore tasks)
        {
            this.tasks = tasks;
        }

        public virtual ITaskInstance Find(long id)
        {
            return this.tasks.Find(id);
        }
    }
}
