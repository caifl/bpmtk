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

        protected override void Activate(ExecutionContext executionContext)
        {
            if (this.LoopCharacteristics != null)
            {
                this.ExecuteLoopActivity(executionContext);
                return;
            }

            base.Activate(executionContext);
        }

        public override void Leave(ExecutionContext executionContext)
        {
            if (this.LoopCharacteristics != null)
            {
                this.LoopCharacteristics.Leave(executionContext);
                return;
            }

            base.Leave(executionContext);
        }

        protected virtual void ExecuteLoopActivity(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            int numberOfInstances = 0;

            try
            {
                numberOfInstances = this.LoopCharacteristics.CreateInstances(executionContext);
            }
            catch (BpmnError error)
            {
                throw error;
                //ErrorPropagation.propagateError(error, execution);
            }

            if (numberOfInstances == 0) //实例数量为零的情况下仍然建立一个活动实例, 只是不执行该节点的任何行为
            {
                this.OnActivating(executionContext);

                this.OnActivated(executionContext);

                //ignore activity behavior.
                //base.Execute(executionContext);

                base.LeaveDefault(executionContext);
            }
        }

        internal virtual void ExecuteInnerActivity(ExecutionContext executionContext,
            IDictionary<string, object> variables)
        {
            base.Activate(executionContext, variables);
        }

        internal virtual void OnInnerActivityEnded(ExecutionContext executionContext)
        {
            base.OnLeave(executionContext);
        }
    }
}
