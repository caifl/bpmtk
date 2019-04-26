using System;

namespace Bpmtk.Bpmn2
{
    public class DataObject : FlowElement, IItemAwareElement
    {
        public virtual DataState DataState
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
