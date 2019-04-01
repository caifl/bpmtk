using System;
using System.Collections.Generic;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;
using Bpmtk.Utils;

namespace Bpmtk.Engine.Runtime
{
    public class ProcessInstance : ExecutionObject, IAggregateRoot
    {
        private Token super;
        private ActivityInstance callActivityInstance;

        private ProcessDefinition processDefinition;
        private ICollection<ProcessVariable> variableInstances;
        private ICollection<InstanceIdentityLink> identityLinks;
        private Token token;
        
        public virtual User Initiator
        {
            get;
            protected set;
        }

        public override IEnumerable<VariableInstance> VariableInstances => this.variableInstances;

        public virtual IEnumerable<IdentityLink> IdentityLinks => this.identityLinks;

        public virtual Token Super
        {
            get => this.super;
            protected set => this.super = value;
        }

        public virtual Token Token
        {
            get => this.token;
            protected set => this.token = value;
        }

        //protected ICollection<Token> tokens;
        protected ProcessInstance()
        { }

        public ProcessInstance(ProcessDefinition processDefinition, 
            Token super = null)
        {
            //this.tokens = new List<Token>();
            this.super = super;
            this.variableInstances = new List<ProcessVariable>();
            this.identityLinks = new List<InstanceIdentityLink>();
            this.processDefinition = processDefinition ?? throw new ArgumentNullException(nameof(processDefinition));

            if (this.super != null)
                this.callActivityInstance = super.ActivityInstance;

            this.State = ExecutionState.Inactive;
            this.Name = processDefinition.Name;
            this.Created = Clock.Now;
            this.Initiator = null;
            this.Description = processDefinition.Description;

            this.InitializeContext();
        }

        protected virtual void InitializeContext()
        {
            //this.processDefinition.Model;
        }

        public virtual ProcessDefinition ProcessDefinition
        {
            get => this.processDefinition;
            protected set => this.processDefinition = value;
        }

        public virtual ActivityInstance CallActivityInstance
        {
            get => this.callActivityInstance;
            protected set => this.callActivityInstance = value;
        }

        public virtual void Start()
        {
            if (this.token != null)
                throw new RuntimeException("The process instance has already started.");

            BpmnActivity initialNode = null;

            this.token = new Token(this, initialNode);

            //fire ProcessStartEvent
            this.OnStart(initialNode);
        }

        protected virtual void OnStart(BpmnActivity initialNode)
        {
            this.StartTime = DateTime.Now;

            // fire the process start event
            if (initialNode != null)
            {
                var executionContext = new ExecutionContext(this.token);
                //processDefinition.fireEvent(Event.EVENTTYPE_PROCESS_START, executionContext);

                //execute the start node
                initialNode.Execute(executionContext);
            }
        }
    }
}
