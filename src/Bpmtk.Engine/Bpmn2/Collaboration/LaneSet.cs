using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public class LaneSet : BaseElement
    {
        private List<Lane> lanes = new List<Lane>();

        public virtual IList<Lane> Lanes => this.lanes;

        public virtual string Name
        {
            get;
            set;
        }
    }

    public class Lane : BaseElement
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual FlowNode FlowNodeRef
        {
            get;
            set;
        }

        public virtual LaneSet ChildLaneSet
        {
            get;
            set;
        }

        public virtual BaseElement PartitionElement
        {
            get;
            set;
        }

        public virtual BaseElement PartitionElementRef
        {
            get;
            set;
        }
    }
}
