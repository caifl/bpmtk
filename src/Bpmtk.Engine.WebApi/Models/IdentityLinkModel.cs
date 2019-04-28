using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class IdentityLinkModel
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual int? UserId
        {
            get;
            set;
        }

        public virtual string UserName
        {
            get;
            set;
        }

        public virtual int? GroupId
        {
            get;
            set;
        }

        public virtual string GroupName
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
