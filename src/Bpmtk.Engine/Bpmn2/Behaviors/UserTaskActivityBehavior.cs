using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class UserTaskActivityBehavior : TaskActivityBehavior, ISignallableActivityBehavior
    {
        //userTask extended-attributes.
        const string TaskName = "name";
        const string TaskPriority = "priority";
        const string Assignee = "assignee";
        const string AssignmentStrategy = "assignmentStrategy";

        public override void Execute(ExecutionContext executionContext)
        {
            var context = executionContext.Context;
            var taskManager = context.TaskManager;

            var taskDef = executionContext.Node as UserTask;
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
            IAssignmentStrategy assignmentStrategry = null;
            var identityManager = context.IdentityManager;
            var evaluator = executionContext.GetEvaluator();

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
                            taskName = evaluator.Evaluate<string>(value);
                            if(taskName != null)
                                builder.SetName(taskName);
                            break;
                        }

                    case Assignee:
                        string userId = evaluator.Evaluate<string>(value);
                        builder.SetAssignee(userId);
                        break;

                    case AssignmentStrategy:
                        assignmentStrategry = taskManager.GetAssignmentStrategy(value);
                        break;
                }              
            }

            if (assignmentStrategry != null)
                assignmentStrategry.ExecuteAsync(builder);

            builder.Build();
        }

        public virtual void Signal(ExecutionContext executionContext, 
            string signalEvent, 
            IDictionary<string, object> signalData)
        {
            var count = executionContext.GetActiveTaskCount();
            if (count > 0)
                throw new RuntimeException($"There are '{count}' tasks need to be completed.");

            base.Leave(executionContext);
        }
    }
}
