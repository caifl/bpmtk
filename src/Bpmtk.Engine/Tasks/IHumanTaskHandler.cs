using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Tasks
{
    public interface IHumanTaskHandler
    {
        void Execute(ExecutionContext executionContext);
    }
}
