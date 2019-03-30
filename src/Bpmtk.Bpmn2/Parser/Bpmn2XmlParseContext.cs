using System;
using System.Collections.Generic;

namespace Bpmtk.Bpmn2.Parser
{
    class Bpmn2XmlParseContext : IParseContext
    {
        private readonly Dictionary<string, Queue<Action<IBaseElement>>> requestQueues = new Dictionary<string, Queue<Action<IBaseElement>>>();
        private readonly Dictionary<string, FlowElement> flowElements = new Dictionary<string, FlowElement>();
        private readonly Dictionary<string, IBaseElement> elements = new Dictionary<string, IBaseElement>();

        public Bpmn2XmlParseContext(Definitions definitions,
            BpmnFactory bpmnFactory)
        {
            Definitions = definitions;
            BpmnFactory = bpmnFactory;
        }

        public virtual Definitions Definitions { get; }

        public virtual BpmnFactory BpmnFactory { get; }

        public virtual void AddReferenceRequest<TBaseElement>(string id, Action<TBaseElement> action)
            where TBaseElement : IBaseElement
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

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
        }
    }
}
