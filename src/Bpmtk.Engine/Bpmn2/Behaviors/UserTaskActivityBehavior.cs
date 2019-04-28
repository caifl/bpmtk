using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class UserTaskActivityBehavior : TaskActivityBehavior, ISignallableActivityBehavior
    {
        const string TaskName = "name";
        const string TaskPriority = "priority";
        const string ActualOwner = "assignee";
        const string AssignmentStrategy = "assignmentStrategy";

        public override async Task ExecuteAsync(ExecutionContext executionContext)
        {
            var taskManager = executionContext.Context.TaskManager;

            var taskDef = executionContext.Node as Bpmtk.Bpmn2.UserTask;
            if (taskDef == null)
                throw new RuntimeException("Invalid task.");

            var taskName = taskDef.Name;
            if (string.IsNullOrEmpty(taskName))
                taskName = taskDef.Id;

            var builder = taskManager.CreateBuilder()
                .SetToken(executionContext.Token)
                .SetActivityId(taskDef.Id)
                .SetName(taskName);

            var attrs = taskDef.Attributes;
            ITaskAssignmentStrategy assignmentStrategry = null;

            var evaluator = executionContext.GetEvalutor();

            foreach (var attr in attrs)
            {
                var value = attr.Value;
                if (string.IsNullOrEmpty(value))
                    continue;

                switch (attr.Name)
                {
                    case TaskPriority: //priority
                        builder.SetPriority(short.Parse(attr.Value));
                        break;

                    case TaskName:
                        {
                            taskName = evaluator.EvaluteToString(value);
                            if(taskName != null)
                                builder.SetName(taskName);
                            break;
                        }

                    case ActualOwner:
                        string userName = executionContext.EvaluteExpression<string>(value);
                        var assignee = await executionContext.Context.IdentityManager.FindUserByNameAsync(userName);
                        if(assignee != null)
                            builder.SetAssignee(assignee);

                        break;

                    case AssignmentStrategy:
                        assignmentStrategry = taskManager.GetTaskAssignmentStrategy(value);
                        break;
                }              
            }

            if (assignmentStrategry != null)
                await assignmentStrategry.ExecuteAsync(builder);

            await builder.BuildAsync();
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
