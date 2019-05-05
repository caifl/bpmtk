using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine
{
    public interface IRuntimeManager
    {
        //IQueryable<ProcessInstance> ProcessInstances
        //{
        //    get;
        //}

        //IQueryable<Token> Tokens
        //{
        //    get;
        //}
        
        /// <summary>
        /// Create process-instance query.
        /// </summary>
        /// <returns></returns>
        IProcessInstanceQuery CreateInstanceQuery();

        //ITokenQuery CreateTokenQuery();

        /// <summary>
        /// Create process-instance builder.
        /// </summary>
        /// <returns>typeof(IProcessInstanceBuilder)</returns>
        IProcessInstanceBuilder CreateInstanceBuilder();

        /// <summary>
        /// Start new process-instance by builder.
        /// </summary>
        Task<IProcessInstance> StartProcessAsync(IProcessInstanceBuilder builder);

        /// <summary>
        /// Start new process-instance by Key of process-definition. 
        /// </summary>
        /// <param name="processDefintionKey">The Key of process-definition</param>
        /// <param name="variables">Initial Variables</param>
        Task<IProcessInstance> StartProcessByKeyAsync(string processDefintionKey,
            IDictionary<string, object> variables = null);

        Task<IProcessInstance> StartProcessByMessageAsync(string messageName,
            IDictionary<string, object> messageData = null);

        Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId);

        Task<IList<Token>> GetActiveTokensAsync(long processInstanceId);

        Task TriggerAsync(long tokenId, IDictionary<string, object> variables = null);

        Task<int> GetActiveTaskCountAsync(long tokenId);

        Task<IProcessInstance> FindAsync(long processInstanceId);

        Task<IDictionary<string, object>> GetVariablesAsync(long processInstanceId,
            string[] variableNames = null);

        Task<IList<Variable>> GetVariableInstancesAsync(long processInstanceId,
            string[] variableNames = null);

        /// <summary>
        /// Update the specified variables of process-instance.
        /// </summary>
        Task SetVariablesAsync(long processInstanceId, IDictionary<string, object> variables);

        /// <summary>
        /// Change the Name of process-instance.
        /// </summary>
        Task SetNameAsync(long processInstanceId, string name);

        /// <summary>
        /// Change the Name of process-instance.
        /// </summary>
        Task SetKeyAsync(long processInstanceId, string key);

        Task<IList<IdentityLink>> GetIdentityLinksAsync(long processInstanceId);

        Task<IList<IdentityLink>> AddUserLinksAsync(long processInstanceId, IEnumerable<int> userIds, string type);

        Task<IList<IdentityLink>> AddGroupLinksAsync(long processInstanceId, IEnumerable<int> groupIds, string type);

        Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds);

        Task<IList<Comment>> GetCommentsAsync(long processInstanceId);

        Task<Comment> AddCommentAsync(long processInstanceId, string comment);

        Task RemoveCommentAsync(long commentId);

        Task SuspendAsync(long processInstanceId,
            string comment = null);

        Task ResumeAsync(long processInstanceId,
            string comment = null);

        Task AbortAsync(long processInstanceId,
            string comment = null);

        Task DeleteAsync(long processInstanceId,
            string comment = null);

        //Task SaveAsync(ProcessInstance processInstance);
    }
}
