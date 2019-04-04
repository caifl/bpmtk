using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    /// <summary>
    /// 接口声明
    /// </summary>
    public class Interface : RootElement
    {
        protected readonly List<Operation> operations = new List<Operation>();

        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 1. C#类或接口全称(charp:[className, assemblyName])
        /// 2. WebApi URI
        /// 3. WebService URI
        /// </summary>
        public virtual string ImplementationRef
        {
            get;
            set;
        }

        public virtual List<Operation> Operations => this.operations;
    }
}
