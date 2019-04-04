using System;

namespace Bpmtk.Engine.Bpmn2
{
    public class DataStore : RootElement
    {
        public DataStore()
        {
            this.IsUnlimited = true;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual int? Capacity
        {
            get;
            set;
        }

        public virtual bool IsUnlimited
        {
            get;
            set;
        }

        public virtual ItemDefinition ItemSubjectRef
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
