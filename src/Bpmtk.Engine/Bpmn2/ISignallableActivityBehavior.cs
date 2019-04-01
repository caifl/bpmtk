using Bpmtk.Engine.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Bpmn2
{
    public interface ISignallableActivityBehavior
    {
        void Signal(ExecutionContext executionContext,
            string signalName, object signalData);
    }
}
