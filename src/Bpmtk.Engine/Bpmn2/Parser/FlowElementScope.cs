using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class FlowElementScope
    {
        private List<KeyValuePair<string, SequenceFlow>> sourceRefs = new List<KeyValuePair<string, SequenceFlow>>();
        private List<KeyValuePair<string, SequenceFlow>> targetRefs = new List<KeyValuePair<string, SequenceFlow>>();
        private List<KeyValuePair<string, Activity>> activityDefaults = new List<KeyValuePair<string, Activity>>();
        private List<KeyValuePair<string, Gateway>> gatewayDefaults = new List<KeyValuePair<string, Gateway>>();
        private Dictionary<string, SequenceFlow> sequenceFlowByIds = new Dictionary<string, SequenceFlow>();
        private Dictionary<string, FlowNode> flowNodeByIds = new Dictionary<string, FlowNode>();

        private List<KeyValuePair<string, FlowNode>> incomings = new List<KeyValuePair<string, FlowNode>>();
        private List<KeyValuePair<string, FlowNode>> outgings = new List<KeyValuePair<string, FlowNode>>();

        public FlowElementScope(IFlowElementsContainer Container)
        {
            this.Container = Container;
        }

        public virtual IFlowElementsContainer Container
        {
            get;
        }

        public virtual void AddSourceRef(string sourceRef, SequenceFlow sequenceFlow)
        {
            this.sourceRefs.Add(new KeyValuePair<string, SequenceFlow>(sourceRef, sequenceFlow));
        }

        public virtual void AddTargetRef(string targetRef, SequenceFlow sequenceFlow)
        {
            this.targetRefs.Add(new KeyValuePair<string, SequenceFlow>(targetRef, sequenceFlow));
        }

        public virtual void AddDefault(string @default, Activity activity) => this.activityDefaults.Add(new KeyValuePair<string, Activity>(@default, activity));

        public virtual void AddDefault(string @default, Gateway gateway) => this.gatewayDefaults.Add(new KeyValuePair<string, Gateway>(@default, gateway));

        public virtual void Add(FlowElement flowElement)
        {
            //flowElement.Container = this.Container;
            //this.Container.FlowElements.Add(flowElement);

            if(flowElement is SequenceFlow)
                this.sequenceFlowByIds.Add(flowElement.Id, flowElement as SequenceFlow);

            else if(flowElement is FlowNode)
                this.flowNodeByIds.Add(flowElement.Id, flowElement as FlowNode);
        }

        public virtual void AddIncoming(string incoming, FlowNode flowNode)
            => this.incomings.Add(new KeyValuePair<string, FlowNode>(incoming, flowNode));

        public virtual void AddOutgoing(string outgoing, FlowNode flowNode)
            => this.outgings.Add(new KeyValuePair<string, FlowNode>(outgoing, flowNode));

        public virtual void Complete()
        {
            FlowNode node = null;
            SequenceFlow sequenceFlow = null;

            foreach(var item in this.sourceRefs)
            {
                if(this.flowNodeByIds.TryGetValue(item.Key, out node))
                    item.Value.SourceRef = node;
            }

            foreach (var item in this.targetRefs)
            {
                if (this.flowNodeByIds.TryGetValue(item.Key, out node))
                    item.Value.TargetRef = node;
            }

            foreach(var item in this.activityDefaults)
            {
                if (this.sequenceFlowByIds.TryGetValue(item.Key, out sequenceFlow))
                    item.Value.Default = sequenceFlow;
            }

            foreach (var item in this.gatewayDefaults)
            {
                if (this.sequenceFlowByIds.TryGetValue(item.Key, out sequenceFlow))
                    item.Value.Default = sequenceFlow;
            }

            foreach (var item in this.incomings)
            {
                if (this.sequenceFlowByIds.TryGetValue(item.Key, out sequenceFlow))
                    item.Value.Incomings.Add(sequenceFlow);
            }

            foreach (var item in this.outgings)
            {
                if (this.sequenceFlowByIds.TryGetValue(item.Key, out sequenceFlow))
                    item.Value.Outgoings.Add(sequenceFlow);
            }
        }
    }
}
