using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public interface IProcessInstanceBuilder
    {
        IList<Bpmtk.Bpmn2.FlowNode> InitialNodes
        {
            get;
        }

        IProcessInstanceBuilder SetProcessDefinition(ProcessDefinition processDefinition);

        IProcessInstanceBuilder SetVariables(IDictionary<string, object> variables);

        IProcessInstanceBuilder SetName(string name);

        IProcessInstanceBuilder SetKey(string key);

        IProcessInstanceBuilder SetInitiator(int initiatorId);

        IProcessInstanceBuilder SetDescription(string description);

        IProcessInstanceBuilder SetSuper(Token super);

        //IProcessInstanceBuilder SetCaller(ActivityInstance caller);

        Task<ProcessInstance> BuildAsync();
    }
}
