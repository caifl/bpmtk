using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Models
{
    public class Group
    {
        public virtual int Id
        {
            get;
            protected set;
        }

        public virtual string Name
        {
            get;
            set;
        }
    }
}
