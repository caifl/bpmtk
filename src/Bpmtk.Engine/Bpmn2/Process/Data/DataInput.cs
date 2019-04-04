using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class DataInput : BaseElement, IItemAwareElement
    {
        public DataInput()
        {
            this.IsCollection = false;
        }

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
