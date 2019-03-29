using System;

namespace Bpmtk.Bpmn2
{
    public class Property : BaseElement, IItemAwareElement
    {
        public virtual DataState DataState
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual ItemDefinition ItemSubjectRef
        {
            get;
            set;
        }

        public virtual bool IsCollection
        {
            get;
            set;
        }
    }
}
