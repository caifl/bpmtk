using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class OutputSet : BaseElement
    {
        public OutputSet()
        {
            this.DataOutputRefs = new List<DataOutput>();
            this.OptionalOutputRefs = new List<DataOutput>();
            this.WhileExecutingOutputRefs = new List<DataOutput>();
            this.InputSetRefs = new List<InputSet>();
        }

        /// <remarks/>
        //[XmlElement("dataOutputRefs", DataType = "IDREF", Order = 0)]
        public virtual ICollection<DataOutput> DataOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("optionalOutputRefs", DataType = "IDREF", Order = 1)]
        public virtual ICollection<DataOutput> OptionalOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("whileExecutingOutputRefs", DataType = "IDREF", Order = 2)]
        public virtual ICollection<DataOutput> WhileExecutingOutputRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlElement("inputSetRefs", DataType = "IDREF", Order = 3)]
        public virtual ICollection<InputSet> InputSetRefs
        {
            get;
        }

        /// <remarks/>
        //[XmlAttribute("name")]
        public virtual string Name
        {
            get;
            set;
        }
    }
}
