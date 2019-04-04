using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2.DC;

namespace Bpmtk.Engine.Bpmn2.DI
{
    public class BPMNEdge : LabeledEdge, ILabeled
    {
        public BPMNEdge()
        {
            this.Waypoints = new List<Point>();
        }

        public virtual IList<Point> Waypoints
        {
            get;
        }

        public virtual BPMNLabel BpmnLabel
        {
            get;
            set;
        }

        public override Label Label { get => this.BpmnLabel; set => this.BpmnLabel = (BPMNLabel)value; }

        public virtual string BpmnElement
        {
            get;
            set;
        }

        public virtual string SourceElement
        {
            get;
            set;
        }

        public virtual string TargetElement
        {
            get;
            set;
        }

        public virtual MessageVisibleKind MessageVisibleKind
        {
            get;
            set;
        }
    }
}
