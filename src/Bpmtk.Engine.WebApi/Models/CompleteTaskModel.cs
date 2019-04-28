using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Models
{
    public class CompleteTaskModel
    {
        public virtual string Comment
        {
            get;
            set;
        }

        public virtual Dictionary<string, object> Variables
        {
            get;
            set;
        }
    }
}
