using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class Event : FlowNode
    {
        protected List<Property> properties = new List<Property>();

        public virtual IList<Property> Properties => this.properties;
    }
}
