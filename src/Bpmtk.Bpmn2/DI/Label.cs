using System;
using Bpmtk.Bpmn2.DC;

namespace Bpmtk.Bpmn2.DI
{
    public abstract class Label : Node
    {
        public virtual Bounds Bounds
        {
            get;
            set;
        }
    }
}
