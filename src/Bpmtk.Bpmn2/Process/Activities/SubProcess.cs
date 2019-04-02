using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Bpmn2
{
    public class SubProcess : Activity, IFlowElementsContainer
    {
        private readonly FlowElementCollection flowElements;
        protected List<Artifact> artifacts = new List<Artifact>();
        private IDictionary<string, FlowElement> flowElementById;

        public SubProcess()
        {
            this.flowElements = new FlowElementCollection(this);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual FlowElement FindFlowElementById(string id, bool recurive = false)
        {
            if (this.flowElementById == null)
            {
                this.flowElementById = this.flowElements.ToDictionary(x => x.Id);
            }

            FlowElement flowElement = null;
            if (this.flowElementById.TryGetValue(id, out flowElement))
                return flowElement;

            if (recurive)
            {
                var subProcessList = this.flowElements.OfType<SubProcess>();
                foreach (var subProcess in subProcessList)
                {
                    flowElement = subProcess.FindFlowElementById(id, recurive);
                    if (flowElement != null)
                        return flowElement;
                }
            }

            return null;
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
