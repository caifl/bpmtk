using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Query;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine
{
    public interface IRuntimeService
    {
        IProcessInstanceQuery CreateProcessInstanceQuery();

        Task<IProcessInstance> FindProcessInstanceByIdAsync(long id);

        Task SetProcessInstanceNameAsync(long id, string name);

        IProcessInstance StartProcessInstanceByMessage(string messageName, object messageData = null);

        IProcessInstance StartProcessInstanceByKey(string processDefinitionKey,
            IDictionary<string, object> variables = null);

        IEnumerable<string> GetActiveActivityIds(long id);
    }
}
