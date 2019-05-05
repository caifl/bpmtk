using System;
using System.Collections.Generic;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine.Models
{
    public class User : IUser
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
