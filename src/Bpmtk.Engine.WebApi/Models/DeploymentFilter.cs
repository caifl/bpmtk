using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Models
{
    public class DeploymentFilter
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Category
        {
            get;
            set;
        }

        public virtual int? PackageId
        {
            get;
            set;
        }

        public virtual int Page
        {
            get;
            set;
        }

        public virtual int PageSize
        {
            get;
            set;
        }
    }
}
