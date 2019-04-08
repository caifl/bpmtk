using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Stores;

namespace Bpmtk.Engine.Bpmn2
{
    public class SubProcess : Activity, IFlowElementsContainer
    {
        private readonly FlowElementCollection flowElements;
        protected List<Artifact> artifacts = new List<Artifact>();

        public SubProcess()
        {
            this.flowElements = new FlowElementCollection(this);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual bool TriggeredByEvent
        {
            get;
            set;
        }

        public virtual IList<FlowElement> FlowElements => this.flowElements;

        public virtual IList<Artifact> Artifacts => this.artifacts;

        protected override void OnActivating(ExecutionContext executionContext)
        {
            base.OnActivating(executionContext);

            var context = executionContext.Context;
            var act = executionContext.ActivityInstance;

            //initialize data-objects.
            act.InitializeContext(context);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var context = executionContext.Context;

            var startEvent = this.FlowElements
                .OfType<StartEvent>()
                .Where(x => x.EventDefinitions.Count == 0 && x.EventDefinitionRefs.Count == 0)
                .FirstOrDefault();

            if (startEvent == null)
                throw new RuntimeException($"No initial activity found for subprocess '{this.Id}'.");

            var token = executionContext.Token;
            var scope = executionContext.ActivityInstance;

            //create sub-process scope token.
            var child = token.CreateToken(context);
            child.Node = startEvent;
            child.Scope = scope;

            var subExecution = ExecutionContext.Create(context, child);
            startEvent.Enter(subExecution);
        }
    }
}
