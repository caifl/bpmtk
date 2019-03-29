using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class DataInput : BaseElement, IItemAwareElement
    {
        public DataInput()
        {
            this.IsCollection = false;
        }

        /// <remarks/>
        //[XmlElement("dataState", Order = 0)]
        public DataState DataState
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("itemSubjectRef")]
        public string ItemSubjectRef
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("isCollection")]
        //[System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsCollection
        {
            get;
            set;
        }
    }
}
