﻿using System;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ProcessInstanceFilter
    {
        public ProcessInstanceFilter()
        {
            this.Page = 1;
            this.PageSize = 20;
        }

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
