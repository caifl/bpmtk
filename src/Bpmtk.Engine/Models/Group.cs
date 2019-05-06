using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Models
{
    public class Group
    {
        public virtual string Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual ICollection<UserGroup> Users
        {
            get;
            set;
        }
    }
}
