using System;

namespace Bpmtk.Engine.Bpmn2.DI
{
    public abstract class LabeledEdge : Edge
    {
        public abstract Label Label
        {
            get;
            set;
        }
    }
}
