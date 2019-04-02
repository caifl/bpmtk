using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2.Choreographies
{
    public class CallChoreography : ChoreographyActivity
    {
        public virtual string CalledChoreographyRef
        {
            get;
            set;
        }

        public virtual IList<ParticipantAssociation> ParticipantAssociations
        {
            get;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
