using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public interface ISignallableActivityBehavior
    {
        Task SignalAsync(ExecutionContext executionContext, string signalEvent,
            IDictionary<string, object> signalData);
    }
}
