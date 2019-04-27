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

        public virtual ProcessDefinition ProcessDefinition
        {
            get;
            set;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            set;
        }

        public virtual TaskInstance Task
        {
            get;
            set;
        }
    }
}
