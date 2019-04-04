using System;
using Bpmtk.Engine.Bpmn2.DC;

namespace Bpmtk.Engine.Bpmn2.DI
{
    public abstract class Shape : Node
    {
        public virtual Bounds Bounds
        {
            get;
            set;
        }
    }
}
