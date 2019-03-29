using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
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

        public string ItemSubjectRef
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
