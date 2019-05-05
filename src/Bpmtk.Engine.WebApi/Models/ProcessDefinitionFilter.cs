using System;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ProcessDefinitionFilter
    {
        public ProcessDefinitionFilter()
        {
            this.Page = 1;
            this.PageSize = 20;
        }

        public virtual ProcessDefinitionState? State
        {
            get;
            set;
        }

        public virtual ProcessDefinitionState[] AnyStates
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
