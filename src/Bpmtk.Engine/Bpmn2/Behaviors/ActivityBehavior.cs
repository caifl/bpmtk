using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public abstract class ActivityBehavior : FlowNodeActivityBehavior
    {
        public virtual LoopActivityBehavior LoopActivityBehavior
        {
            get;
            set;
        }

        protected override SequenceFlow GetDefaultOutgoing(ExecutionContext executionContext)
        {
            var activity = executionContext.Node as Activity;
            return activity.Default;
        }

        public override void Leave(ExecutionContext executionContext)
        {
            if (this.LoopActivityBehavior != null)
            {
                this.LoopActivityBehavior.Leave(executionContext);
                return;
            }

            base.Leave(executionContext);
        }
    }
}
