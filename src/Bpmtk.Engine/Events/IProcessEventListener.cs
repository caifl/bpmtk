using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public interface IProcessEventListener
    {
        Task StartedAsync(IExecutionContext executionContext);

        Task EnterNodeAsync(IExecutionContext executionContext);

        Task ActivatedAsync(IExecutionContext executionContext);

        Task LeaveNodeAsync(IExecutionContext executionContext);

        Task TakeTransitionAsync(IExecutionContext executionContext);

        Task EndedAsync(IExecutionContext executionContext);
    }
}
