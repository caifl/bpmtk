using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2.Choreographies
{
    public abstract class ChoreographyActivity : FlowNode
    {
        public virtual string InitiatingParticipantRef
        {
            get;
            set;
        }

        public virtual ChoreographyLoopType LoopType
        {
            get;
            set;
        }

        public virtual IList<string> ParticipantRefs
        {
            get;
        }

        public virtual IList<CorrelationKey> CorrelationKeys
        {
            get;
        }
    }
}
