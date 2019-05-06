using System;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public interface IProcessEventListener
    {
        void ProcessStart(IExecutionContext executionContext);

        void ActivityReady(IExecutionContext executionContext);

        void ActivityStart(IExecutionContext executionContext);

        void ActivityEnd(IExecutionContext executionContext);

        void TakeTransition(IExecutionContext executionContext);

        void ProcessEnd(IExecutionContext executionContext);
    }
}
