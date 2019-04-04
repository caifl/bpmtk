using System;

namespace Bpmtk.Engine.Bpmn2
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
