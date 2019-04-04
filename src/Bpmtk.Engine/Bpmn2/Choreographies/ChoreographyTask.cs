using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2.Choreographies
{
    public class ChoreographyTask : ChoreographyActivity
    {
        public virtual IList<string> MessageFlowRefs
        {
            get;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
