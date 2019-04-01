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
            protected set;
        }

        public virtual Group Group
        {
            get;
            protected set;
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

    public class InstanceIdentityLink : IdentityLink
    {
    }

    public class ActivityIdentityLink : IdentityLink
    {
    }

    public class DefinitionIdentityLink : IdentityLink
    {
    }
}
