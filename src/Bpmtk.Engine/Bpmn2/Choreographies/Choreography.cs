using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2.Choreographies
{
    public class Choreography : Collaboration
    {
        public Choreography()
        {
            this.FlowElements = new List<FlowElement>();
        }

        public virtual IList<FlowElement> FlowElements
        {
            get;
        }
    }
}
