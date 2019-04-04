using System;
using Bpmtk.Engine.Bpmn2.DC;

namespace Bpmtk.Engine.Bpmn2.DI
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
