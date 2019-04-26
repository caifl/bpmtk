using System;

namespace Bpmtk.Bpmn2
{
    public class ParticipantAssociation : BaseElement
    {
        public virtual string InnerParticipantRef
        {
            get;
            set;
        }

        public virtual string OuterParticipantRef
        {
            get;
            set;
        }
    }
}
