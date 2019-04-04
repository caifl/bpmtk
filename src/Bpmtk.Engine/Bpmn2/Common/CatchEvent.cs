using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class CatchEvent : Event
    {
        protected List<DataOutput> dataOutputs = new List<DataOutput>();
        protected List<DataOutputAssociation> dataOutputAssociations = new List<DataOutputAssociation>();
        protected List<EventDefinition> eventDefinitionRefs = new List<EventDefinition>();
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

        public virtual IList<EventDefinition> EventDefinitionRefs => this.eventDefinitionRefs;

        public virtual bool ParallelMultiple
        {
            get;
            set;
        }
    }
}
