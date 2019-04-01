using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class BpmnActivityBehavior : FlowNodeBehavior
    {
        //public FlowNodeBehavior(FlowNode node)
        //{
        //}
        public virtual MultiInstanceActivityBehavior MultiInstanceActivityBehavior
        {
            get;
            set;
        }

        public override void Leave(ExecutionContext executionContext)
        {
            if (this.MultiInstanceActivityBehavior != null)
                this.MultiInstanceActivityBehavior.Leave(executionContext);

            base.Leave(executionContext);
        }

        //public virtual void Leave(ExecutionContext executionContext)
        //{
            

        //}
    }
}
