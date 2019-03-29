using System;

namespace Bpmtk.Bpmn2.DI
{
    public class BPMNShape : LabeledShape, ILabeled
    {
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

        public virtual bool IsHorizontal
        {
            get;
            set;
        }

        public virtual bool IsExpanded
        {
            get;
            set;
        }

        public virtual bool IsMarkerVisible
        {
            get;
            set;
        }

        public virtual bool IsMessageVisible
        {
            get;
            set;
        }

        public virtual ParticipantBandKind ParticipantBandKind
        {
            get;
            set;
        }

        public virtual string ChoreographyActivityShape
        {
            get;
            set;
        }
    }
}
