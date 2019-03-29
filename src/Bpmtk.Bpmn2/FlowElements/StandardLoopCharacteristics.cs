using System;

namespace Bpmtk.Bpmn2
{
    public class StandardLoopCharacteristics : LoopCharacteristics
    {
        public StandardLoopCharacteristics()
        {
            this.TestBefore = false;
        }

        /// <summary>
        /// A boolean Expression that controls the loop. The Activity will only loop
        //as long as this condition is true. The looping behavior MAY be
        //underspecified, meaning that the modeler can simply document the
        //condition, in which case the loop cannot be formally executed.
        /// </summary>
        public virtual Expression LoopCondition
        {
            get;
            set;
        }

        /// <summary>
        /// Flag that controls whether the loop condition is evaluated at the beginning
        // (testBefore = true) or at the end(testBefore = false) of the loop
        // iteration.
        /// </summary>
        public virtual bool TestBefore
        {
            get;
            set;
        }

        /// <summary>
        /// Serves as a cap on the number of iterations.
        /// </summary>
        public virtual int? LoopMaximum
        {
            get;
            set;
        }
    }
}
