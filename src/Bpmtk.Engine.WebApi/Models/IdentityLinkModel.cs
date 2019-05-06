using System;

namespace Bpmtk.Engine.WebApi.Models
{
    public class IdentityLinkModel
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string UserId
        {
            get;
            set;
        }

        public virtual string GroupId
        {
            get;
            set;
        }

        public virtual string Type
        {
            get;
            set;
        }
    }
}
