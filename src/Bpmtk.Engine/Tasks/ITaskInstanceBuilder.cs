using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskInstanceBuilder
    {
        IContext Context
        {
            get;
        }

        ITaskInstanceBuilder SetToken(Token token);

        ITaskInstanceBuilder SetActivityId(string activityId);

        ITaskInstanceBuilder SetPriority(short priority);

        ITaskInstanceBuilder SetName(string name);

        ITaskInstanceBuilder SetAssignee(User assignee);

        ITaskInstanceBuilder SetDueDate(DateTime dueDate);

        Task<TaskInstance> BuildAsync();
    }
}
