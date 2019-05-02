using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public interface IFlowNodeActivityBehavior
    {
        /// <summary>
        /// When an activity becomes ready this does not mean that the activity immediately starts. Instead, 
        /// it will remain ready to perform until all the defined preconditions (such as data inputs and messages) 
        /// become satisfied.
        /// </summary>
        Task<bool> EvaluatePreConditionsAsync(ExecutionContext executionContext);

        Task ExecuteAsync(ExecutionContext executionContext);

        Task LeaveAsync(ExecutionContext executionContext);
    }
}
