using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskInstanceBuilder
    {
        ITaskInstanceBuilder SetUserTask(UserTask userTask);

        IList<TaskInstance> Build(ExecutionContext executionContext);
    }
}
