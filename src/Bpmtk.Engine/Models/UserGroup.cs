using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Models
{
    public class UserGroup
    {
        public virtual string UserId
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual string GroupId
        {
            get;
            set;
        }

        public virtual Group Group
        {
            get;
            set;
        }
    }
}
