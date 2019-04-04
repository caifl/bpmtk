using System;

namespace Bpmtk.Engine.Bpmn2.Choreographies
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
