using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class GlobalUserTask : GlobalTask
    {
        public virtual IList<Rendering> Renderings
        {
            get;
        }

        public virtual string Implementation
        {
            get;
            set;
        }
    }
}
