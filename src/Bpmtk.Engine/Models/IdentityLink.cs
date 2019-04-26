using System;

namespace Bpmtk.Engine.Models
{
    public class IdentityLink
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual Group Group
        {
            get;
            set;
        }

        public virtual string Type
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }
    }
}
