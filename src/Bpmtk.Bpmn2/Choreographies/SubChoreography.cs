using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2.Choreographies
{
    public class SubChoreography : ChoreographyActivity
    {
        public SubChoreography()
        {
            this.FlowElements = new List<FlowElement>();
            this.Artifacts = new List<Artifact>();
        }

        public virtual IList<FlowElement> FlowElements
        {
            get;
        }

        public virtual IList<Artifact> Artifacts
        {
            get;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
