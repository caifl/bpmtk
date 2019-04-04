using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class DataOutput : BaseElement, IItemAwareElement
    {
        public DataOutput()
        {
            this.IsCollection = false;
        }

        public DataState DataState
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public virtual ItemDefinition ItemSubjectRef
        {
            get;
            set;
        }

        public bool IsCollection
        {
            get;
            set;
        }
    }
}
