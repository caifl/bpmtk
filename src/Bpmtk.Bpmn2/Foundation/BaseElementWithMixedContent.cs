using System;

namespace Bpmtk.Bpmn2
{
    public abstract class BaseElementWithMixedContent
    {
        public virtual string Documentation
        {
            get;
            set;
        }

        public virtual ExtensionElements ExtensionElements
        {
            get;
            set;
        }

        public virtual string Text
        {
            get;
            set;
        }

        public virtual string Id
        {
            get;
            set;
        }
    }
}
