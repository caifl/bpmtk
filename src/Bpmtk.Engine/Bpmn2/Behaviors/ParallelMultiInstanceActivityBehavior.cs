using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class ParallelMultiInstanceActivityBehavior : MultiInstanceActivityBehavior
    {
        public ParallelMultiInstanceActivityBehavior(ActivityBehavior innerActivityBehavior, MultiInstanceLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior, loopCharacteristics)
        {
        }

        protected override int CreateInstances(ExecutionContext executionContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
