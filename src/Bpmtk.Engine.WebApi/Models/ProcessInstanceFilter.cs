using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ProcessInstanceFilter
    {
        public virtual ExecutionState? State
        {
            get;
            set;
        }

        public virtual ExecutionState[] AnyStates
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
