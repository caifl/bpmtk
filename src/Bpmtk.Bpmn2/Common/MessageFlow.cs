using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    /// <remarks/>
    //[XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    //[XmlRoot("messageFlow", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class MessageFlow : BaseElement
    {
        //private string name;

        //private string sourceRef;

        //private string targetRef;

        //private string messageRef;

        /// <remarks/>
        //[XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("sourceRef")]
        public string SourceRef
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("targetRef")]
        public string TargetRef
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("messageRef")]
        public string MessageRef
        {
            get;
            set;
        }
    }

    /// <remarks/>    
    
    //[XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL")]
    //[XmlRoot("messageFlowAssociation", Namespace = "http://www.omg.org/spec/BPMN/20100524/MODEL", IsNullable = false)]
    public class MessageFlowAssociation : BaseElement
    {
        //private string innerMessageFlowRef;

        //private string outerMessageFlowRef;

        /// <remarks/>
        //[XmlAttribute("innerMessageFlowRef")]
        public string InnerMessageFlowRef
        {
            get;
            set;
        }

        /// <remarks/>
        //[XmlAttribute("outerMessageFlowRef")]
        public string OuterMessageFlowRef
        {
            get;
            set;
        }
    }
    
    //[XmlType(Namespace = "http://www.omg.org/spec/BPMN/20100524/DI")]
    public enum MessageVisibleKind
    {
        //[XmlEnum("initiating")]
        /// <remarks/>
        Initiating,

        /// <remarks/>
        //[XmlEnum("non_initiating")]
        NonInitiating,
    }
}
