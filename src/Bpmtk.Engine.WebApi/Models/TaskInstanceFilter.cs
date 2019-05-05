using System;

namespace Bpmtk.Engine.WebApi.Models
{
    public class TaskInstanceFilter
    {
        public TaskInstanceFilter()
        {
            this.Page = 1;
            this.PageSize = 20;
        }

        public virtual int? AssigneeId
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
