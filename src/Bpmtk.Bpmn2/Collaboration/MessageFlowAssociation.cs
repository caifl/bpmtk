using System;

namespace Bpmtk.Bpmn2
{
    public class MessageFlowAssociation : BaseElement
    {
        public string InnerMessageFlowRef
        {
            get;
            set;
        }

        public string OuterMessageFlowRef
        {
            get;
            set;
        }
    }
}
