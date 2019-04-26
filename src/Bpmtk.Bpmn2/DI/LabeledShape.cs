using System;

namespace Bpmtk.Bpmn2.DI
{
    public abstract class LabeledShape : Shape
    {
        public abstract Label Label
        {
            get;
            set;
        }
    }
}
