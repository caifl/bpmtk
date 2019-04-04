using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class Assignment : BaseElement
    {
        public virtual Expression From
        {
            get;
            set;
        }

        public virtual Expression To
        {
            get;
            set;
        }
    }
}
