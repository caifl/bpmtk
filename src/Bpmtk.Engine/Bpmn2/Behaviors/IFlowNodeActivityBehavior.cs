using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public interface IFlowNodeActivityBehavior
    {
        Task<bool> CanActivateAsync(ExecutionContext executionContext,
            IList<Token> joinedTokens);

        Task ExecuteAsync(ExecutionContext executionContext);

        Task LeaveAsync(ExecutionContext executionContext);
    }
}
