using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class DataAssociation : BaseElement
    {
        protected readonly List<string> sourceRefs;
        protected readonly List<Assignment> assignments;

        public DataAssociation()
        {
            this.sourceRefs = new List<string>();
            this.assignments = new List<Assignment>();
        }

        public virtual IList<string> SourceRefs => this.sourceRefs;

        public virtual string TargetRef
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
