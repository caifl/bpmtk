using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class StandardLoopActivityBehavior : LoopActivityBehavior
    {
        private readonly StandardLoopCharacteristics loopCharacteristics;

        public StandardLoopActivityBehavior(ActivityBehavior innerActivityBehavior, StandardLoopCharacteristics loopCharacteristics) : base(innerActivityBehavior)
        {
            this.loopCharacteristics = loopCharacteristics;
        }
    }
}
