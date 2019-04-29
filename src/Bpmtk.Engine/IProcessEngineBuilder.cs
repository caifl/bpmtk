using System;
using Microsoft.Extensions.Logging;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Events;

namespace Bpmtk.Engine
{
    public interface IProcessEngineBuilder
    {
        IProcessEngineBuilder SetContextFactory(IContextFactory contextFactory);

        IProcessEngineBuilder SetLoggerFactory(ILoggerFactory loggerFactory);

        IProcessEngineBuilder AddProcessEventListener(IProcessEventListener processEventListener);

        IProcessEngineBuilder AddTaskEventListener(ITaskEventListener taskEventListener);

        IProcessEngineBuilder AddTaskAssignmentStrategy(string key,
            string name, IAssignmentStrategy taskAssignmentStrategy);

        IProcessEngine Build();
    }
}
