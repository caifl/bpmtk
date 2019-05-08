using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Variables;
using System.Threading.Tasks;

namespace Bpmtk.Engine
{
    public interface IRuntimeManager
    {   
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

        ///// <summary>
        ///// Start new process-instance by Key of process-definition. 
        ///// </summary>
        ///// <param name="processDefintionKey">The Key of process-definition</param>
        ///// <param name="variables">Initial Variables</param>
        //Task<IProcessInstance> StartProcessByKeyAs(string processDefintionKey,
        //    IDictionary<string, object> variables = null);

        /// <summary>
        /// Start new process-instance by Key of process-definition. 
        /// </summary>
        /// <param name="processDefintionKey">The Key of process-definition</param>
        /// <param name="variables">Initial Variables</param>
        Task<IProcessInstance> StartProcessByKeyAsync(string processDefintionKey,
            IDictionary<string, object> variables = null);

        Task<IProcessInstance> StartProcessByMessageAsync(string messageName,
            IDictionary<string, object> messageData = null);

        IList<string> GetActiveActivityIds(long processInstanceId);

        Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId);

        IList<IToken> GetActiveTokens(long processInstanceId);

        Task<IList<IToken>> GetActiveTokensAsync(long processInstanceId);

        void Trigger(long tokenId, IDictionary<string, object> variables = null);

        Task TriggerAsync(long tokenId, IDictionary<string, object> variables = null);

        int GetActiveTaskCount(long tokenId);

        IProcessInstance Find(long processInstanceId);

        Task<IProcessInstance> FindAsync(long processInstanceId);

        IDictionary<string, object> GetVariables(long processInstanceId, string[] variableNames = null);

        Task<IDictionary<string, object>> GetVariablesAsync(long processInstanceId, string[] variableNames = null);

        IList<IVariable> GetVariableInstances(long processInstanceId, string[] variableNames = null);

        /// <summary>
        /// Update the specified variables of process-instance.
        /// </summary>
        void SetVariables(long processInstanceId, IDictionary<string, object> variables);

        /// <summary>
        /// Change the Name of process-instance.
        /// </summary>
        Task<IProcessInstance> SetNameAsync(long processInstanceId, string name);

        /// <summary>
        /// Change the Name of process-instance.
        /// </summary>
        Task<IProcessInstance> SetKeyAsync(long processInstanceId, string key);

        Task<IList<IdentityLink>> GetIdentityLinksAsync(long processInstanceId);

        Task<IList<IdentityLink>> AddUserLinksAsync(long processInstanceId, IEnumerable<string> userIds, string type);

        Task<IList<IdentityLink>> AddGroupLinksAsync(long processInstanceId, IEnumerable<string> groupIds, string type);

        Task RemoveIdentityLinksAsync(long processInstanceId, params long[] identityLinkIds);

        Task<IList<Comment>> GetCommentsAsync(long processInstanceId);

        Task<Comment> AddCommentAsync(long processInstanceId, string comment);

        void RemoveComment(long commentId);

        void Suspend(long processInstanceId,
            string comment = null);

        void Resume(long processInstanceId,
            string comment = null);

        void Abort(long processInstanceId,
            string comment = null);

        void Delete(long processInstanceId,
            string comment = null);

        //void SaveAsync(ProcessInstance processInstance);
    }
}
