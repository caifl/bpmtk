using System;

namespace Bpmtk.Engine.Models
{
    public abstract class IdentityLink
    {
        public virtual long Id
        {
            get;
            protected set;
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

        public virtual int Ordinal
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
