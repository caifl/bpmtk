using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class StandardLoopActivityBehavior : LoopActivityBehavior
    {
        private readonly StandardLoopCharacteristics loopCharacteristics;

        public StandardLoopActivityBehavior(ActivityBehavior innerActivityBehavior, StandardLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior)
        {
            this.loopCharacteristics = loopCharacteristics;
        }

        protected override Task<int> CreateInstancesAsync(ExecutionContext executionContext)
        {
            throw new NotImplementedException();
        }
    }
}
