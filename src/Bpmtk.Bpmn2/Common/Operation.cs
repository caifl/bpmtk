using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class Operation : BaseElement
    {
        protected List<string> errorRefs = new List<string>();

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
        public string InMessageRef
        {
            get;
            set;
        }

        /// <summary>
        /// message#id
        /// </summary>
        public string OutMessageRef
        {
            get;
            set;
        }

        public virtual IList<string> ErrorRefs => this.errorRefs;
    }
}
