using System;

namespace Bpmtk.Bpmn2
{
    public class MessageFlow : BaseElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string SourceRef
        {
            get;
            set;
        }

        public virtual string TargetRef
        {
            get;
            set;
        }

        public virtual string MessageRef
        {
            get;
            set;
        }
    }

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
