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

        public void Complete(long id, IDictionary<string, object> variables = null)
        {
            var task = this.tasks.Find(id);
            if (task == null)
                throw new Exception("Task not found.");

            task.Complete(Context.Current, variables);
        }

        public ITaskQuery CreateQuery() => this.tasks.CreateQuery();

        public virtual ITaskInstance Find(long id)
        {
            return this.tasks.Find(id);
        }
    }
}
