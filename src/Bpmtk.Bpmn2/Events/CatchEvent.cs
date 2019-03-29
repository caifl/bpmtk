using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class CatchEvent : Event, IHasDataOutputs, IHasDataOutputAssociations
    {
        protected List<DataOutput> dataOutputs = new List<DataOutput>();
        protected List<DataOutputAssociation> dataOutputAssociations = new List<DataOutputAssociation>();
        protected List<string> eventDefinitionRefs = new List<string>();
        protected List<EventDefinition> eventDefinitions = new List<EventDefinition>();

        public CatchEvent()
        {
            this.dataOutputs = new List<DataOutput>();
            this.dataOutputAssociations = new List<DataOutputAssociation>();
        }

        public virtual IList<DataOutput> DataOutputs => this.dataOutputs;

        public virtual IList<DataOutputAssociation> DataOutputAssociations => this.dataOutputAssociations;

        public virtual OutputSet OutputSet
        {
            get;
            set;
        }

        public virtual IList<EventDefinition> EventDefinitions => this.eventDefinitions;

        public virtual IList<string> EventDefinitionRefs => this.eventDefinitionRefs;

        public virtual bool ParallelMultiple
        {
            get;
            set;
        }
    }
}
