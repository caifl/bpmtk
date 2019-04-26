using System;

namespace Bpmtk.Bpmn2.DI
{
    public class BPMNDiagram : Diagram
    {
        public virtual BPMNPlane BPMNPlane
        {
            get;
            set;
        }

        public virtual BPMNLabelStyle BPMNLabelStyle
        {
            get;
            set;
        }
    }
}
