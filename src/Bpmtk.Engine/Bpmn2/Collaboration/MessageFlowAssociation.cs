using System;

namespace Bpmtk.Engine.Bpmn2
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
