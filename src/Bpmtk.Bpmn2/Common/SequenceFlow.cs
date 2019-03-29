using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    public class SequenceFlow : FlowElement
    {
        public virtual Expression ConditionExpression
        {
            get;
            set;
        }

        protected List<EventListener> eventListeners = new List<EventListener>();

        public virtual IList<EventListener> EventListeners => this.eventListeners;

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
