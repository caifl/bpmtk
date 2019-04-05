using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Tasks.Internal
{
    public class HumanTaskHandler : IHumanTaskHandler
    {
        public virtual void Execute(ExecutionContext executionContext)
        {
            if (executionContext == null)
                throw new ArgumentNullException(nameof(executionContext));

            var token = executionContext.Token;
            var userTask = token.Node as UserTask;
            if (userTask == null)
                throw new InvalidOperationException("Invalid userTask.");

            string taskName = userTask.Name;
            if (string.IsNullOrEmpty(taskName))
                taskName = userTask.Id;

            var taskInstance = new TaskInstance(token);
            taskInstance.Name = taskName;
            if (userTask.Documentations.Count > 0)
            {
                var textArray = userTask.Documentations.Select(x => x.Text).ToArray();
                taskInstance.Description = StringHelper.Join(textArray, "\n", 255);
            }

            var store = executionContext.Context.GetService<ITaskStore>();
            store.Add(taskInstance);
        }
    }
}
