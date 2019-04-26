using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class SubProcess : Activity, IFlowElementsContainer
    {
        private readonly FlowElementCollection flowElements;
        protected List<Artifact> artifacts = new List<Artifact>();

        public SubProcess()
        {
            this.flowElements = new FlowElementCollection(this);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual bool TriggeredByEvent
        {
            get;
            set;
        }

        public virtual IList<FlowElement> FlowElements => this.flowElements;

        public virtual IList<Artifact> Artifacts => this.artifacts;
    }
}
