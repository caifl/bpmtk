using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstanceQuery
    {
        IProcessInstanceQuery SetId(long id);

        IProcessInstanceQuery SetKey(string key);

        IProcessInstanceQuery SetName(string name);

        IProcessInstanceQuery SetState(ExecutionState state);

        IProcessInstanceQuery SetStateAny(params ExecutionState[] stateArray);

        IProcessInstanceQuery SetDeploymentId(int deploymentId);

        IProcessInstanceQuery SetCategory(string category);

        IProcessInstanceQuery SetProcessDefinitionId(int processDefinitionId);

        IProcessInstanceQuery SetProcessDefinitionKey(string processDefinitionKey);

        IProcessInstanceQuery SetInitiator(int initiatorId);

        IProcessInstanceQuery SetStartTimeFrom(DateTime fromDate);

        IProcessInstanceQuery SetStartTimeTo(DateTime toDate);

        IProcessInstanceQuery FetchInitiator();

        IProcessInstanceQuery FetchProcessDefinition();

        IProcessInstanceQuery FetchVariables();

        IProcessInstanceQuery FetchIdentityLinks();

        //IProcessInstanceQuery SetReadOnly();

        Task<ProcessInstance> SingleAsync();

        Task<int> CountAsync();

        Task<IList<ProcessInstance>> ListAsync(int page = 1, int pageSize = 20);

        Task<IList<ProcessInstance>> ListAsync();
    }
}
