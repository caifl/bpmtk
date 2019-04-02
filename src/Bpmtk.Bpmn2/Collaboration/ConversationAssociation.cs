using System;

namespace Bpmtk.Bpmn2
{
    public class ConversationAssociation : BaseElement
    {
        public virtual string InnerConversationNodeRef
        {
            get;
            set;
        }

        public virtual string OuterConversationNodeRef
        {
            get;
            set;
        }
    }
}
