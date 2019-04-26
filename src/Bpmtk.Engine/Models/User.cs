using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Models
{
    public class User
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

        public virtual string UserName
        {
            get;
            set;
        }

        public virtual ICollection<UserGroup> Groups
        {
            get;
            set;
        }
    }
}
