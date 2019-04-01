using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Runtime;
using System;

namespace Bpmtk.Engine
{
    public class ExecutionService
    {
        public ExecutionService()
        {
            
        }

        public ProcessInstance StartProcessByKey(string processDefinitionKey)
        {
            var repository = TKContext.GetService<IExecutionRepository>();

            var pi = new ProcessInstance(null);
            pi.Name = "ffff";

            repository.Add(pi);

            return pi;
        }
    }
}
