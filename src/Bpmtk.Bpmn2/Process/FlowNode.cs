using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2
{
    public abstract class FlowNode : FlowElement
    {
        protected List<SequenceFlow> incomings = new List<SequenceFlow>();
        protected List<SequenceFlow> outgoings = new List<SequenceFlow>();

        protected readonly Dictionary<string, string> attributes = new Dictionary<string, string>();
        protected readonly List<EventListener> eventListeners = new List<EventListener>();

        public virtual IList<SequenceFlow> Incomings => this.incomings;

        public virtual IList<SequenceFlow> Outgoings => this.outgoings;

        /// <summary>
        /// Get extended attributes of flow node.
        /// </summary>
        public virtual IDictionary<string, string> Attributes
        {
            get
            {
                return this.attributes;
            }
        }

        /// <summary>
        /// Gets the event listeners of flow node.
        /// </summary>
        public virtual IList<EventListener> EventListeners => this.eventListeners;

        public override string ToString()
        {
            return $"{this.GetType().Name}, {this.Id}, {this.Name}";
        }

        public abstract void Accept(IFlowNodeVisitor visitor);

        public override int GetHashCode()
        {
            if (this.Id != null)
                return this.GetType().Name.GetHashCode() ^ this.Id.GetHashCode();

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var flowNode = obj as FlowNode;
            if (flowNode != null)
                return string.Compare(flowNode.Id, this.Id) == 0;

            return base.Equals(obj);
        }
    }
}
