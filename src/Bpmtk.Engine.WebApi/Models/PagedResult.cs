using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bpmtk.Engine.WebApi.Models
{
    public class PagedResult<TEntity>
    {
        public virtual IList<TEntity> Items
        {
            get;
            set;
        }

        public virtual int Count
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
