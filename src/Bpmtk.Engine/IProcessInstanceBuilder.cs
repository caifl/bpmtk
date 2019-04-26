using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IProcessInstanceBuilder
    {
        IProcessInstanceBuilder SetProcessDefinition(ProcessDefinition processDefinition);

        Task<ProcessInstance> BuildAsync();
    }
}
