using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class ThrowEvent : Event, IHasDataInputs, IHasDataInputAssociations
    {
        protected List<DataInput> dataInputs = new List<DataInput>();
        protected List<DataInputAssociation> dataInputAssociations = new List<DataInputAssociation>();
        protected List<EventDefinition> eventDefinitionRefs = new List<EventDefinition>();
        protected List<EventDefinition> eventDefinitions = new List<EventDefinition>();

        public virtual IList<DataInput> DataInputs => this.dataInputs;

        public virtual IList<DataInputAssociation> DataInputAssociations => this.dataInputAssociations;

        public virtual InputSet InputSet
        {
            get;
            set;
        }

        public virtual List<EventDefinition> EventDefinitions => this.eventDefinitions;

        public virtual List<EventDefinition> EventDefinitionRefs => this.eventDefinitionRefs;
    }
}
