using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class Event : FlowNode, IPropertyContainer
    {
        protected List<Property> properties = new List<Property>();

        public virtual IList<Property> Properties => this.properties;
    }
}
