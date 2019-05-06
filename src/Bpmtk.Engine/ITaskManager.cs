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

        /// <summary>
        /// Find the specified task-instance.
        /// </summary>
        /// <param name="taskId">task identifier.</param>
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

        Task<IList<Comment>> GetCommentsAsync(long taskId);

        Task RemoveCommentsAsync(params long[] commentIds);

        Task<ITaskInstance> SetNameAsync(long taskId, string name);

        Task<ITaskInstance> SetPriorityAsync(long taskId, short priority);

        Task<IList<IdentityLink>> GetIdentityLinksAsync(long taskId);

        Task<IList<IdentityLink>> AddUserLinksAsync(long taskId, IEnumerable<string> userIds, string type);

        Task<IList<IdentityLink>> AddGroupLinksAsync(long taskId, IEnumerable<string> groupIds, string type);

        Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds);

        /// <summary>
        /// Gets registered task assignment strategy entries.
        /// </summary>
        IReadOnlyList<AssignmentStrategyEntry> GetAssignmentStrategyEntries();
    }
}
