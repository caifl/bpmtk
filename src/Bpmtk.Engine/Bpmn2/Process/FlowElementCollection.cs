using System;
using System.Collections;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2
{
    class FlowElementCollection : IList<FlowElement>
    {
        private readonly IFlowElementsContainer container;
        private readonly List<FlowElement> items = new List<FlowElement>();

        public FlowElementCollection(IFlowElementsContainer container)
        {
            this.container = container;
        }

        public virtual FlowElement this[int index]
        {
            get => this.items[index];
            set {
                this.items[index] = value;
                if (value != null)
                    value.Container = this.container;
            }
        }

        public virtual int Count => this.items.Count;

        public virtual bool IsReadOnly => false;

        public virtual void Add(FlowElement item)
        {
            item.Container = this.container;
            this.items.Add(item);
        }

        public virtual void Clear()
        {
            this.items.Clear();
        }

        public virtual bool Contains(FlowElement item) => this.items.Contains(item);

        public virtual void CopyTo(FlowElement[] array, int arrayIndex)
            => this.items.CopyTo(array, arrayIndex);

        public virtual IEnumerator<FlowElement> GetEnumerator()
            => this.items.GetEnumerator();

        public virtual int IndexOf(FlowElement item)
            => this.items.IndexOf(item);

        public void Insert(int index, FlowElement item)
        {
            item.Container = this.container;
            this.items.Insert(index, item);
        }

        public bool Remove(FlowElement item)
            => this.items.Remove(item);

        public void RemoveAt(int index)
            => this.items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
            => this.items.GetEnumerator();
    }
}
