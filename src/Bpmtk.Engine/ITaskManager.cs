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

        Task<TaskInstance> FindAsync(long taskId);

        Task<TaskInstance> ClaimAsync(long taskId, string comment = null);

        Task<TaskInstance> AssignAsync(long taskId, 
            int assigneeId,
            string comment = null);

        Task<TaskInstance> SuspendAsync(long taskId, string comment = null);

        Task<TaskInstance> ResumeAsync(long taskId, string comment = null);

        Task CompleteAsync(long taskId, 
            IDictionary<string, object> variables = null,
            string comment = null);

        Task SetNameAsync(long taskId, string name);

        Task SetPriorityAsync(long taskId, short priority);

        //Task RemoveAsync(long taskId);

        //IReadOnlyList<AssignmentStrategyEntry> GetAssignmentStrategyEntries();

        //IAssignmentStrategy GetAssignmentStrategy(string key);
    }
}
