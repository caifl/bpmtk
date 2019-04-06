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
        private IDictionary<string, FlowElement> flowElementById;

        public SubProcess()
        {
            this.flowElements = new FlowElementCollection(this);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual FlowElement FindFlowElementById(string id, bool recurive = false)
        {
            if (this.flowElementById == null)
            {
                this.flowElementById = this.flowElements.ToDictionary(x => x.Id);
            }

            FlowElement flowElement = null;
            if (this.flowElementById.TryGetValue(id, out flowElement))
                return flowElement;

            if (recurive)
            {
                var subProcessList = this.flowElements.OfType<SubProcess>();
                foreach (var subProcess in subProcessList)
                {
                    flowElement = subProcess.FindFlowElementById(id, recurive);
                    if (flowElement != null)
                        return flowElement;
                }
            }

            return null;
        }

        public virtual bool TriggeredByEvent
        {
            get;
            set;
        }

        public virtual IList<FlowElement> FlowElements => this.flowElements;

        public virtual IList<Artifact> Artifacts => this.artifacts;

        protected override void OnActivate(ExecutionContext executionContext)
        {
            //base.OnActivate(executionContext);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            var context = executionContext.Context;

            IList<FlowNode> initialNodes = this.FlowElements
                .OfType<FlowNode>()
                .Where(x => x.Incomings.Count == 0)
                .ToList();

            if (initialNodes.Count == 0)
                throw new BpmnError("子流程没有起始节点");

            var token = executionContext.Token;
            var scope = executionContext.ActivityInstance;

            var act = executionContext.ActivityInstance;
            act.InitializeContext(context);

            base.OnActivate(executionContext);

            var list = new List<Token>();
            foreach(var initialNode in initialNodes)
            {
                var child = token.CreateToken(context);
                child.Node = initialNode;
                child.Scope = scope;

                list.Add(child);
            }

            var store = executionContext.Context.GetService<IInstanceStore>();

            foreach (var subToken in list)
            {
                var subExecution = ExecutionContext.Create(context, subToken);
                var node = subToken.Node;

                //CreateActivityInstance
                subExecution.ActivityInstance = ActivityInstance.Create(subExecution);

                //processDefinition.fireEvent(Event.EVENTTYPE_PROCESS_START, executionContext);
                
                store.Add(new HistoricToken(subExecution, "start"));

                node.Execute(subExecution);
            }

            //base.Execute(executionContext);
        }

        public override void Leave(ExecutionContext executionContext)
        {
            var token = executionContext.Token;
            if (token.Children.Count > 0)
                throw new BpmnError("该子流程还有环节未完成");

            base.Leave(executionContext);
        }
    }
}
