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

        ITaskQuery CreateQuery();

        ITaskInstance Find(long taskId);

        Task<ITaskInstance> FindAsync(long taskId);

        ITaskInstance Claim(long taskId, string comment = null);

        ITaskInstance Assign(long taskId, 
            string assignee,
            string comment = null);

        ITaskInstance Suspend(long taskId, string comment = null);

        ITaskInstance Resume(long taskId, string comment = null);

        ITaskInstance Complete(long taskId, 
            IDictionary<string, object> variables = null,
            string comment = null);

        Task<ITaskInstance> CompleteAsync(long taskId,
            IDictionary<string, object> variables = null,
            string comment = null);

        ITaskInstance SetName(long taskId, string name);

        ITaskInstance SetPriority(long taskId, short priority);

        //Task RemoveAsync(long taskId);

        IReadOnlyList<AssignmentStrategyEntry> GetAssignmentStrategyEntries();

        IAssignmentStrategy GetAssignmentStrategy(string key);
    }
}
