using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    public class SequenceFlow : FlowElement, IScriptEnabledElement
    {
        protected List<Script> scripts = new List<Script>();

        public virtual Expression ConditionExpression
        {
            get;
            set;
        }

        public virtual IList<Script> Scripts => this.scripts;

        public virtual FlowNode SourceRef
        {
            get;
            set;
        }

        public virtual FlowNode TargetRef
        {
            get;
            set;
        }

        public bool? IsImmediate
        {
            get;
            set;
        }


        public override string ToString()
        {
            return $"{this.Id}, {this.Name}";
        }
    }
}
