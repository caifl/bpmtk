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
        IQueryable<ProcessInstance> ProcessInstances
        {
            get;
        }

        IQueryable<Token> Tokens
        {
            get;
        }

        ITokenQuery CreateTokenQuery();

        IProcessInstanceBuilder CreateProcessInstanceBuilder();

        Task<ProcessInstance> StartProcessAsync(IProcessInstanceBuilder builder);

        Task<ProcessInstance> StartProcessByKeyAsync(string processDefintionKey,
            IDictionary<string, object> variables = null);

        Task<ProcessInstance> StartProcessByMessageAsync(string messageName,
            IDictionary<string, object> messageData = null);

        Task<IList<string>> GetActiveActivityIdsAsync(long processInstanceId);
 
        Task<int> GetActiveTaskCountAsync(long tokenId);

        Task<ProcessInstance> FindAsync(long processInstanceId);

        Task<IDictionary<string, object>> GetVariablesAsync(long processInstanceId,
            string[] variableNames = null);

        Task<IList<Variable>> GetVariableInstancesAsync(long processInstanceId,
            string[] variableNames = null);

        Task SetVariablesAsync(long processInstanceId,
            IDictionary<string, object> variables);

        Task SetNameAsync(long processInstanceId,
            string name);

        Task SuspendAsync(long processInstanceId,
            string comment = null);

        Task ResumeAsync(long processInstanceId,
            string comment = null);

        Task AbortAsync(long processInstanceId,
            string comment = null);

        Task DeleteAsync(long processInstanceId,
            string comment = null);

        Task SaveAsync(ProcessInstance processInstance);
    }
}
