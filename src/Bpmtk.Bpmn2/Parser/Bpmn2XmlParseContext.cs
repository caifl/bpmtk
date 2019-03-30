using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2.Parser
{
    class Bpmn2XmlParseContext : IParseContext
    {
        private readonly Dictionary<string, Stack<Action<object>>> requestStacks = new Dictionary<string, Stack<Action<object>>>();
        private readonly Dictionary<string, ItemDefinition> itemDefinitions = new Dictionary<string, ItemDefinition>();
        private readonly Dictionary<string, Message> messages = new Dictionary<string, Message>();
        private readonly Dictionary<string, FlowElement> flowElements = new Dictionary<string, FlowElement>();
        //protected Stack<FlowElementScope> scopes = new Stack<FlowElementScope>();

        public Bpmn2XmlParseContext(Definitions definitions,
            BpmnFactory bpmnFactory)
        {
            Definitions = definitions;
            BpmnFactory = bpmnFactory;
        }

        public virtual Definitions Definitions { get; }

        public virtual BpmnFactory BpmnFactory { get; }

        //public virtual void AddReferenceRequest(ObjectReferenceRequest value)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException(nameof(value));

        //    IList<ObjectReferenceRequest> list = null;
        //    if (!this.objectRefRequests.TryGetValue(value.Id, out list))
        //    {
        //        list = new List<ObjectReferenceRequest>();
        //        this.objectRefRequests.Add(value.Id, list);
        //    }

        //    list.Add(value);
        //}

        public virtual void AddReferenceRequest<TObject>(string id, Action<TObject> action)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Stack<Action<object>> stack = null;
            if (!this.requestStacks.TryGetValue(id, out stack))
            {
                stack = new Stack<Action<object>>();
                this.requestStacks.Add(id, stack);
            }

            var r = new Action<object>(x => action((TObject)x));
            stack.Push(r);
        }

        public virtual void Push(ItemDefinition value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.itemDefinitions.Add(value.Id, value);

            Stack<Action<object>> stack = null;
            if(this.requestStacks.TryGetValue(value.Id, out stack))
            {
                while(stack.Count > 0)
                {
                    var action = stack.Pop();
                    action(value);
                }

                this.requestStacks.Remove(value.Id);
            }
        }

        public virtual void Push(Message value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.messages.Add(value.Id, value);

            Stack<Action<object>> stack = null;
            if (this.requestStacks.TryGetValue(value.Id, out stack))
            {
                while (stack.Count > 0)
                {
                    var action = stack.Pop();
                    action(value);
                }

                this.requestStacks.Remove(value.Id);
            }
        }

        public virtual void Push(FlowElement value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.flowElements.Add(value.Id, value);

            Stack<Action<object>> stack = null;
            if (this.requestStacks.TryGetValue(value.Id, out stack))
            {
                while (stack.Count > 0)
                {
                    var action = stack.Pop();
                    action(value);
                }

                this.requestStacks.Remove(value.Id);
            }
        }

        public virtual void Complete()
        {

        }

        //public virtual void PushScope(FlowElementScope scope)
        //{
        //    this.scopes.Push(scope);
        //}

        //public virtual FlowElementScope PeekScope() => this.scopes.Peek();

        //public virtual FlowElementScope PopScope() => this.scopes.Pop();
    }

    public class ObjectReferenceRequest
    {
        public ObjectReferenceRequest(string id,
            Action<object> callback)
        {
            Id = id;
            Callback = callback;
        }

        public virtual string Id
        {
            get;
        }

        public virtual Action<object> Callback
        {
            get;
        }
    }
}
