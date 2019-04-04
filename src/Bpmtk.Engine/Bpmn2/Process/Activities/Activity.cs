using System;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public abstract class Activity : FlowNode
    {
        protected List<Property> properties = new List<Property>();
        protected List<DataInputAssociation> dataInputAssociations = new List<DataInputAssociation>();
        protected List<DataOutputAssociation> dataOutputAssociations = new List<DataOutputAssociation>();
        protected List<ResourceRole> resources = new List<ResourceRole>();

        /// <summary>
        /// The activity input/output specification.
        /// </summary>
        public virtual InputOutputSpecification IOSpecification
        {
            get;
            set;
        }

        public virtual IList<Property> Properties => this.properties;

        public virtual IList<DataInputAssociation> DataInputAssociations => this.dataInputAssociations;

        public virtual IList<DataOutputAssociation> DataOutputAssociations => this.dataOutputAssociations;

        public virtual IList<ResourceRole> Resources => this.resources;

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

        public override void Enter(ExecutionContext executionContext)
        {
            if(this.LoopCharacteristics != null)
            {
                this.LoopCharacteristics.Execute(executionContext);
                return;
            }

            base.Enter(executionContext);
        }

        //public override void Execute(ExecutionContext executionContext)
        //{
        //    base.Execute(executionContext);
        //}

        public override void Leave(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            if (token.Parent != null &&
                token.Parent.IsLoopActivity &&
                this.LoopCharacteristics != null)
            {
                this.LoopCharacteristics.Leave(executionContext);
                return;
            }

            base.Leave(executionContext);
        }
    }
}
