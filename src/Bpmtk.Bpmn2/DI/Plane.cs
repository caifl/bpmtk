using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2.DI
{
    public abstract class Plane : Node
    {
        public Plane()
        {
            this.DiagramElements = new List<DiagramElement>();
        }

        public virtual IList<DiagramElement> DiagramElements
        {
            get;
        }
    }
}
