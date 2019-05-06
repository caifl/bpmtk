using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        IProcessInstanceQuery SetInitiator(string initiator);

        IProcessInstanceQuery SetStartTimeFrom(DateTime fromDate);

        IProcessInstanceQuery SetStartTimeTo(DateTime toDate);

        //IProcessInstanceQuery FetchInitiator();

        IProcessInstanceQuery FetchProcessDefinition();

        IProcessInstanceQuery FetchVariables();

        IProcessInstanceQuery FetchIdentityLinks();

        //IProcessInstanceQuery SetReadOnly();

        IProcessInstance Single();

        Task<IProcessInstance> SingleAsync();

        Task<int> CountAsync();

        Task<IList<IProcessInstance>> ListAsync(int page = 1, int pageSize = 20);

        Task<IList<IProcessInstance>> ListAsync();
    }
}
