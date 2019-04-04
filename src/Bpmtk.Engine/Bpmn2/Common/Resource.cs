using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class Resource : RootElement
    {
        protected List<ResourceParameter> parameters = new List<ResourceParameter>();

        /// <summary>
        /// The name of resource. (required)
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The type of resource.(extended)
        /// </summary>
        //public virtual ResourceType? Type
        //{
        //    get;
        //    set;
        //}

        public virtual IList<ResourceParameter> Parameters => this.parameters;
    }

    public class ResourceParameter : BaseElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The type of parameter.
        /// </summary>
        public virtual ItemDefinition Type
        {
            get;
            set;
        }

        public bool? IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// The initial value of parameter.(extended)
        /// </summary>
        //public virtual string Value
        //{
        //    get;
        //    set;
        //}
    }
}
