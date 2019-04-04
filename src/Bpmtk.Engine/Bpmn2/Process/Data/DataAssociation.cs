using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class DataAssociation : BaseElement
    {
        protected readonly List<IItemAwareElement> sourceRefs;
        protected readonly List<Assignment> assignments;

        public DataAssociation()
        {
            this.sourceRefs = new List<IItemAwareElement>();
            this.assignments = new List<Assignment>();
        }

        public virtual IList<IItemAwareElement> SourceRefs => this.sourceRefs;

        public virtual IItemAwareElement TargetRef
        {
            get;
            set;
        }

        public virtual FormalExpression Transformation
        {
            get;
            set;
        }

        public virtual IList<Assignment> Assignments => this.assignments;
    }
}
