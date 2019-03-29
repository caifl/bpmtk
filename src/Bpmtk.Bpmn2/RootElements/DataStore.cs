using System;

namespace Bpmtk.Bpmn2
{
    public class DataStore : RootElement
    {
        public DataStore()
        {
            this.IsUnlimited = true;
        }

        public string Name
        {
            get;
            set;
        }

        public int? Capacity
        {
            get;
            set;
        }

        public bool IsUnlimited
        {
            get;
            set;
        }

        public string ItemSubjectRef
        {
            get;
            set;
        }

        public DataState DataState
        {
            get;
            set;
        }
    }
}
