using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Tasks
{
    public interface IHumanTaskHandler
    {
        Task ExecuteAsync(ExecutionContext executionContext);
    }
}
