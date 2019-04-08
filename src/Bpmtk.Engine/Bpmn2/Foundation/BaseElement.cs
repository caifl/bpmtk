using Bpmtk.Engine.Bpmn2.Extensions;
using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class BaseElement : IBaseElement
    {
        protected List<Documentation> documentations = new List<Documentation>();
        protected List<ExtendedAttribute> attributes = new List<ExtendedAttribute>();

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

        public virtual IList<ExtendedAttribute> Attributes
        {
            get => this.attributes;
        }

        public virtual IList<Documentation> Documentations => this.documentations;

        public override string ToString()
        {
            if (this.Id != null)
                return $"{this.GetType().Name} [{this.Id}]";

            return base.ToString();
        }
    }

    public interface IBaseElement
    {
        string Id
        {
            get;
        }
    }
}
