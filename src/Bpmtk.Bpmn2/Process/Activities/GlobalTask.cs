using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class GlobalTask : CallableElement
    {
        public GlobalTask()
        {
            this.Resources = new List<ResourceRole>();
        }

        public virtual IList<ResourceRole> Resources
        {
            get;
        }
    }
}
