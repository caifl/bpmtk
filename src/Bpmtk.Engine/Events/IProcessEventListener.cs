using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Events
{
    public interface IProcessEventListener
    {
        Task ProcessStartAsync(IExecutionContext executionContext);

        Task ActivityReadyAsync(IExecutionContext executionContext);

        Task ActivityStartAsync(IExecutionContext executionContext);

        Task ActivityEndAsync(IExecutionContext executionContext);

        Task TakeTransitionAsync(IExecutionContext executionContext);

        Task ProcessEndAsync(IExecutionContext executionContext);
    }
}
