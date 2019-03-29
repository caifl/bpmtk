using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class BaseElement
    {
        protected List<Documentation> documentations = new List<Documentation>();

        /// <remarks/>
        //[XmlElement(Order = 1)]
        public virtual ExtensionElements ExtensionElements
        {
            get;
            set;
        }

        public virtual string Id
        {
            get;
            set;
        }

        public virtual IList<Documentation> Documentations => this.documentations;

        public override string ToString()
        {
            if (this.Id != null)
                return $"{this.GetType().Name} [{this.Id}]";

            return base.ToString();
        }
    }
}
