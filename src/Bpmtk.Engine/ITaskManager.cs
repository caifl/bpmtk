using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine
{
    public interface ITaskManager
    {
        ITaskInstanceBuilder CreateBuilder();

        IQueryable<TaskInstance> Tasks
        {
            get;
        }

        ITaskQuery CreateQuery();

        Task CreateAsync(TaskInstance task);

        Task<TaskInstance> FindTaskAsync(long taskId);

        Task CompleteAsync(long taskId, 
            IDictionary<string, object> variables = null);

        Task UpdateAsync(TaskInstance task);

        Task RemoveAsync(TaskInstance task);

        ITaskAssignmentStrategy GetTaskAssignmentStrategy(string name);
    }
}
