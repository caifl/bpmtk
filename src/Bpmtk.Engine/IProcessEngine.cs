using System;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine
{
    public interface IProcessEngine
    {
        ProcessEngineOptions Options
        {
            get;
        }

        ILoggerFactory LoggerFactory
        {
            get;
        }

        //IProcessEventListener ProcessEventListener
        //{
        //    get;
        //}

        //ITaskEventListener TaskEventListener
        //{
        //    get;
        //}

        //IAssignmentStrategy GetTaskAssignmentStrategy(string key);

        //IReadOnlyList<AssignmentStrategyEntry> GetTaskAssignmentStrategyEntries();

        IContext CreateContext();

        bool TryGetValue(string name, out object value);

        object GetValue(string name, object defaultValue = null);

        TValue GetValue<TValue>(string name, TValue defaultValue = default(TValue));

        IProcessEngine SetValue(string name, object value);
    }
}
