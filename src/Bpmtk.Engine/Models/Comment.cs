using System;

namespace Bpmtk.Engine.Models
{
    public class Comment
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual int UserId
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual string Body
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
