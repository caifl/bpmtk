using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class Operation : BaseElement
    {
        protected List<Error> errorRefs = new List<Error>();

        public string Name
        {
            get;
            set;
        }

        public virtual Interface Interface
        {
            get;
            set;
        }

        /// <summary>
        /// 如C#函数名, 或者webService方法名称, 或者http method(get,post,delete,put)
        /// </summary>
        public string ImplementationRef
        {
            get;
            set;
        }

        /// <summary>
        /// message#id
        /// </summary>
        public virtual Message InMessageRef
        {
            get;
            set;
        }

        /// <summary>
        /// message#id
        /// </summary>
        public virtual Message OutMessageRef
        {
            get;
            set;
        }

        public virtual IList<Error> ErrorRefs => this.errorRefs;
    }
}
