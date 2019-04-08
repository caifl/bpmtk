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

            string value = null;
            foreach (var attr in userTask.Attributes)
            {
                switch(attr.Name)
                {
                    case "assignee":
                        value = attr.Value;
                        if(!string.IsNullOrEmpty(value))
                        {
                            var v = executionContext.EvaluteExpression(value);
                            if(v != null)
                            {
                                var idStore = executionContext.Context.GetService<IIdentityStore>();
                                taskInstance.Assignee = idStore.FindUserByName(v.ToString());
                                taskInstance.AssigneeId = taskInstance.Assignee.Id;
                            }
                        }
                        break;

                    case "name":
                        value = attr.Value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            var v = executionContext.EvaluteExpression(value);
                            if (v != null)
                                taskInstance.Name = v.ToString();
                        }
                        break;

                    case "priority":
                        value = attr.Value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            short p = 0;
                            if (short.TryParse(value, out p))
                                taskInstance.Priority = p;
                        }
                        break;
                }
            }

            var store = executionContext.Context.GetService<ITaskStore>();
            store.Add(taskInstance);
        }
    }
}
