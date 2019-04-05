using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Utils;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Runtime
{
    public class ActivityInstance : ExecutionObject, IAggregateRoot
    {
        private ICollection<ActivityVariable> variableInstances;
        private ICollection<ActivityIdentityLink> identityLinks;
        private FlowNode activity;
        protected ICollection<ActivityInstance> children;

        protected ActivityInstance()
        {
        }

        public ActivityInstance(ProcessInstance processInstance,
            FlowNode activity,
            ActivityInstance parent = null)
        {
            this.Parent = parent;
            this.children = new List<ActivityInstance>();

            this.ProcessInstance = processInstance;
            this.activity = activity;

            this.ActivityId = activity.Id;
            this.ActivityType = activity.GetType().Name;

            //Set initial state.
            this.State = ExecutionState.Ready;
            this.Created = Clock.Now;

            this.Name = activity.Name;
            if (string.IsNullOrEmpty(this.Name))
                this.Name = activity.Id;

            if (activity.Documentations.Count > 0)
            {
                var textArray = activity.Documentations.Select(x => x.Text).ToArray();
                this.Description = StringHelper.Join(textArray, "\n", 255);
            }
        }

        public virtual ProcessInstance ProcessInstance
        {
            get;
            protected set;
        }

        public virtual ActivityInstance Parent
        {
            get;
            protected set;
        }

        public virtual ProcessInstance SubProcessInstance
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            protected set;
        }

        public virtual string ActivityType
        {
            get;
            protected set;
        }

        public virtual ICollection<ActivityInstance> Children
        {
            get => this.children;
        }

        public override IEnumerable<VariableInstance> VariableInstances => this.variableInstances;

        public virtual void Activate()
        {
            this.State = ExecutionState.Active;
            this.StartTime = Clock.Now;
        }

        public virtual void Finish()
        {
            this.State = ExecutionState.Completed;
            this.EndTime = Clock.Now;
        }

        public static ActivityInstance Create(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            var node = token.Node;
            ActivityInstance act = null;
            var processInstance = executionContext.ProcessInstance;
            var scope = executionContext.Scope;
            act = new ActivityInstance(processInstance, node, scope);
            if (scope != null)
                scope.Children.Add(act);

            var store = executionContext.Context.GetService<IInstanceStore>();
            store.Add(act);

            return act;
        }
    }
}
