using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class UserTaskActivityBehavior : TaskActivityBehavior, ISignallableActivityBehavior
    {
        public override async Task ExecuteAsync(ExecutionContext executionContext)
        {
            var taskManager = executionContext.Context.TaskManager;

            var taskDef = executionContext.Node as Bpmtk.Bpmn2.UserTask;
            if (taskDef == null)
                throw new RuntimeException("Invalid task.");

            var taskInstance = new TaskInstance();

            var name = taskDef.Name;
            if (string.IsNullOrEmpty(name))
                name = taskDef.Id;

            var date = Clock.Now;

            taskInstance.ActivityId = taskDef.Id;
            taskInstance.Created = date;
            taskInstance.State = TaskState.Ready;
            taskInstance.LastStateTime = date;
            taskInstance.Modified = date;
            taskInstance.Name = taskDef.Name;
            taskInstance.ActivityInstance = executionContext.ActivityInstance;
            taskInstance.ProcessInstance = executionContext.ProcessInstance;
            taskInstance.Token = executionContext.Token;

            await taskManager.CreateAsync(taskInstance);
            //await executionContext.Context.DbSession.FlushAsync();
        }

        public async Task SignalAsync(ExecutionContext executionContext, 
            string signalEvent, 
            IDictionary<string, object> signalData)
        {
            var count = await executionContext.GetActiveTaskCountAsync();
            if (count > 0)
                throw new RuntimeException($"There are '{count}' tasks need to be completed.");

            await base.LeaveAsync(executionContext);
        }
    }
}
