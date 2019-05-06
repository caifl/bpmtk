using System;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public interface ISignallableActivityBehavior
    {
        void Signal(ExecutionContext executionContext, string signalEvent,
            IDictionary<string, object> signalData);
    }
}
