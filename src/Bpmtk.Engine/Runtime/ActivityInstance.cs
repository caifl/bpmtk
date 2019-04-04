using System;
using System.Collections.Generic;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public class ActivityInstance : ExecutionObject, IAggregateRoot
    {
        private ICollection<ActivityVariable> variableInstances;
        private ICollection<ActivityIdentityLink> identityLinks;
        private ICollection<Token> tokens;
        protected ProcessInstance processInstance;
        private FlowNode activity;

        public ActivityInstance(ProcessInstance processInstance,
            FlowNode activity)
        {
            this.processInstance = processInstance;
            this.activity = activity;

            this.ActivityId = activity.Id;
            this.Name = activity.Name;
            //this.Description = activity.Description;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public override IEnumerable<VariableInstance> VariableInstances => this.variableInstances;

        public virtual ProcessInstance ProcessInstance => this.processInstance;

        public ActivityInstance(Token token)
        {
            this.tokens = new List<Token>();
            this.tokens.Add(token);
        }

        public ActivityInstance()
        {
        }

        public virtual void Activate()
        {

        }
    }
}
