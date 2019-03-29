﻿using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2
{
    public abstract class Activity : FlowNode, IPropertyContainer, 
        IResourceRoleContainer, 
        IHasIOSpecification,
        IHasDataInputAssociations,
        IHasDataOutputAssociations
    {
        protected List<Property> properties = new List<Property>();
        protected List<DataInputAssociation> dataInputAssociations = new List<DataInputAssociation>();
        protected List<DataOutputAssociation> dataOutputAssociations = new List<DataOutputAssociation>();
        protected List<ResourceRole> resourceRoles = new List<ResourceRole>();

        /// <summary>
        /// The activity input/output specification.
        /// </summary>
        public virtual IOSpecification IOSpecification
        {
            get;
            set;
        }

        public virtual IList<Property> Properties => this.properties;

        public virtual IList<DataInputAssociation> DataInputAssociations => this.dataInputAssociations;

        public virtual IList<DataOutputAssociation> DataOutputAssociations => this.dataOutputAssociations;

        public virtual IList<ResourceRole> ResourceRoles => this.resourceRoles;

        public virtual LoopCharacteristics LoopCharacteristics
        {
            get;
            set;
        }

        public virtual bool IsForCompensation
        {
            get;
            set;
        }

        public virtual int StartQuantity
        {
            get;
            set;
        }

        public virtual int CompletionQuantity
        {
            get;
            set;
        }

        public virtual SequenceFlow Default
        {
            get;
            set;
        }
    }
}