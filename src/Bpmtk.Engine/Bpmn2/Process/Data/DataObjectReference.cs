using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class DataObjectReference : FlowElement, IItemAwareElement
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

        public virtual DataObject DataObjectRef
        {
            get;
            set;
        }
    }
}
