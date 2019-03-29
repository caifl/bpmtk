using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public interface IFlowElementsContainer
    {
        IList<FlowElement> FlowElements
        {
            get;
        }

        IList<Artifact> Artifacts
        {
            get;
        }

        FlowElement FindFlowElementById(string id, bool recurive = false);
    }
}
