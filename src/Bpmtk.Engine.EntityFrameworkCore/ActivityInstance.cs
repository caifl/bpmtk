using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public class ActivityInstance
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual ICollection<ActivityVariable> Variables
        {
            get;
            set;
        }
    }
}
