using System;

namespace Bpmtk.Bpmn2.Choreographies
{
    public class GlobalChoreographyTask : Choreography
    {
        public virtual string InitiatingParticipantRef
        {
            get;
            set;
        }
    }
}
