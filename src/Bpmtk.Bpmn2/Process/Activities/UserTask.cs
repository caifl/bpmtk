using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public class UserTask : Task
    {
        protected List<Rendering> renderings = new List<Rendering>();

        public UserTask()
        {
            this.Implementation = "##unspecified";
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual IList<Rendering> Renderings => this.renderings;

        public virtual string Implementation
        {
            get;
            set;
        }
    }
}
