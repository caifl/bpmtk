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

        public virtual string Assignee
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
