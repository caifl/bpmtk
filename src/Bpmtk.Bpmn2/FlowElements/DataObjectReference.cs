using System;

namespace Bpmtk.Bpmn2
{
    public class DataObjectReference : FlowElement
    {
        public virtual DataState DataState
        {
            get;
            set;
        }

        public virtual string ItemSubjectRef
        {
            get;
            set;
        }

        public virtual string DataObjectRef
        {
            get;
            set;
        }
    }
}
