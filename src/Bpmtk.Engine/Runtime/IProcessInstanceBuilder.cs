using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstanceBuilder
    {
        IProcessInstanceBuilder SetProcessDefinitionId(int processDefinitionId);

        IProcessInstanceBuilder SetProcessDefinitionKey(string processDefinitionKey);

        IProcessInstanceBuilder SetVariables(IDictionary<string, object> variables);

        IProcessInstanceBuilder SetName(string name);

        IProcessInstanceBuilder SetKey(string key);

        IProcessInstanceBuilder SetInitiator(string initiator);

        IProcessInstanceBuilder SetDescription(string description);

        IProcessInstanceBuilder SetSuper(Token super);

        IProcessInstance Build();
    }
}
