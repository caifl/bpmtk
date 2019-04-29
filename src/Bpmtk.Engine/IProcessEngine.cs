using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Events;

namespace Bpmtk.Engine
{
    public interface IProcessEngine
    {
        ILoggerFactory LoggerFactory
        {
            get;
        }

        IProcessEventListener ProcessEventListener
        {
            get;
        }

        ITaskEventListener TaskEventListener
        {
            get;
        }

        IAssignmentStrategy GetTaskAssignmentStrategy(string key);

        IReadOnlyList<AssignmentStrategyEntry> GetTaskAssignmentStrategyEntries();

        IContext CreateContext();
    }
}
