using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Models
{
    public class Group
    {
        public virtual int Id
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
