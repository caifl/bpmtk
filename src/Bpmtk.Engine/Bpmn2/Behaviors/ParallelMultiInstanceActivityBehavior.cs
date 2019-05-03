using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class ParallelMultiInstanceActivityBehavior : MultiInstanceActivityBehavior
    {
        public ParallelMultiInstanceActivityBehavior(ActivityBehavior innerActivityBehavior, MultiInstanceLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior, loopCharacteristics)
        {
        }

        protected override Task<int> CreateInstancesAsync(ExecutionContext executionContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
