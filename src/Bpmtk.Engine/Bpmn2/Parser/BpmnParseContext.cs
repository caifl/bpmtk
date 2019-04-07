using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    class BpmnParseContext : IParseContext
    {
        private readonly Dictionary<string, Queue<Action<IBaseElement>>> requestQueues = new Dictionary<string, Queue<Action<IBaseElement>>>();
        private readonly Dictionary<string, FlowElement> flowElements = new Dictionary<string, FlowElement>();
        private readonly Dictionary<string, IBaseElement> elements = new Dictionary<string, IBaseElement>();
        private readonly Dictionary<string, List<SequenceFlow>> sourceRefs = new Dictionary<string, List<SequenceFlow>>();
        private readonly Dictionary<string, List<SequenceFlow>> targetRefs = new Dictionary<string, List<SequenceFlow>>();
        private readonly List<FlowNode> flowNodes = new List<FlowNode>();

        public BpmnParseContext(Definitions definitions,
            BpmnFactory bpmnFactory)
        {
            Definitions = definitions;
            BpmnFactory = bpmnFactory;
        }

        public virtual IDictionary<string, FlowElement> FlowElements => this.flowElements;

        public virtual Definitions Definitions { get; }

        public virtual BpmnFactory BpmnFactory { get; }

        public virtual void AddReferenceRequest<TBaseElement>(string id, Action<TBaseElement> action)
            where TBaseElement : IBaseElement
        {
            if (id == null)
                return;
                //throw new ArgumentNullException(nameof(id));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            IBaseElement value = null;
            if (elements.TryGetValue(id, out value))
            {
                action((TBaseElement)value);
                return;
            }

            Queue<Action<IBaseElement>> queue = null;
            if (!this.requestQueues.TryGetValue(id, out queue))
            {
                queue = new Queue<Action<IBaseElement>>();
                this.requestQueues.Add(id, queue);
            }

            var r = new Action<IBaseElement>(x => action((TBaseElement)x));
            queue.Enqueue(r);
        }

        //public virtual void Push(ItemDefinition value)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException(nameof(value));

        //    this.itemDefinitions.Add(value.Id, value);
        //    this.elements.Add(value.Id, value);

        //    Queue<Action<object>> queue = null;
        //    if(this.requestQueues.TryGetValue(value.Id, out queue))
        //    {
        //        while(queue.Count > 0)
        //        {
        //            var action = queue.Dequeue();
        //            action(value);
        //        }

        //        this.requestQueues.Remove(value.Id);
        //    }
        //}

        //public virtual void Push(Message value)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException(nameof(value));

        //    this.messages.Add(value.Id, value);
        //    this.elements.Add(value.Id, value);

        //    Queue<Action<object>> queue = null;
        //    if (this.requestQueues.TryGetValue(value.Id, out queue))
        //    {
        //        while (queue.Count > 0)
        //        {
        //            var action = queue.Dequeue();
        //            action(value);
        //        }

        //        this.requestQueues.Remove(value.Id);
        //    }
        //}
        public virtual void AddFlowNode(FlowNode flowNode)
        {
            this.flowNodes.Add(flowNode);
        }

        public virtual void Push(FlowElement value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.flowElements.Add(value.Id, value);
            this.elements.Add(value.Id, value);

            Queue<Action<IBaseElement>> queue = null;
            if (this.requestQueues.TryGetValue(value.Id, out queue))
            {
                while (queue.Count > 0)
                {
                    var action = queue.Dequeue();
                    action(value);
                }

                this.requestQueues.Remove(value.Id);
            }
        }

        public virtual void Push(BaseElement baseElement)
        {
            if (baseElement == null)
                throw new ArgumentNullException(nameof(baseElement));

            if (string.IsNullOrEmpty(baseElement.Id))
                return;

            this.elements.Add(baseElement.Id, baseElement);

            Queue<Action<IBaseElement>> queue = null;
            if (this.requestQueues.TryGetValue(baseElement.Id, out queue))
            {
                while (queue.Count > 0)
                {
                    var action = queue.Dequeue();
                    action(baseElement);
                }

                this.requestQueues.Remove(baseElement.Id);
            }
        }

        public virtual void Complete()
        {
            var em = this.requestQueues.GetEnumerator();
            while(em.MoveNext())
            {
                var key = em.Current.Key;
                var queue = em.Current.Value;

                IBaseElement value = null;
                if (!elements.TryGetValue(key, out value))
                    continue;

                while (queue.Count > 0)
                {
                    var action = queue.Dequeue();
                    action(value);
                }
            }

            this.requestQueues.Clear();

            var flowNodes = this.flowNodes;//this.flowElements.Values.OfType<FlowNode>();
            List<SequenceFlow> flows = null;
            foreach(var flowNode in flowNodes)
            {
                var id = flowNode.Id;
                if(this.sourceRefs.TryGetValue(id, out flows))
                {
                    foreach (var sf in flows)
                    {
                        if(!flowNode.Outgoings.Contains(sf))
                            flowNode.Outgoings.Add(sf);
                    }
                }

                if (this.targetRefs.TryGetValue(id, out flows))
                {
                    foreach (var sf in flows)
                    {
                        if (!flowNode.Incomings.Contains(sf))
                            flowNode.Incomings.Add(sf);
                    }
                }
            }
        }

        public void AddSourceRef(string sourceRef, SequenceFlow sequenceFlow)
        {
            List<SequenceFlow> flows = null;
            if (!this.sourceRefs.TryGetValue(sourceRef, out flows))
            {
                flows = new List<SequenceFlow>();
                this.sourceRefs.Add(sourceRef, flows);
            }

            flows.Add(sequenceFlow);
        }

        public void AddTargetRef(string targetRef, SequenceFlow sequenceFlow)
        {
            List<SequenceFlow> flows = null;
            if (!this.targetRefs.TryGetValue(targetRef, out flows))
            {
                flows = new List<SequenceFlow>();
                this.targetRefs.Add(targetRef, flows);
            }

            flows.Add(sequenceFlow);
        }
    }
}
