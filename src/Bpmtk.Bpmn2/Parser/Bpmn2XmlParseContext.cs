using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2.Parser
{
    class Bpmn2XmlParseContext : IParseContext
    {
        protected Stack<FlowElementScope> scopes = new Stack<FlowElementScope>();

        public Bpmn2XmlParseContext(Definitions definitions,
            BpmnFactory bpmnFactory)
        {
            Definitions = definitions;
            BpmnFactory = bpmnFactory;
        }

        public Definitions Definitions { get; }

        public BpmnFactory BpmnFactory { get; }

        public virtual void PushScope(FlowElementScope scope)
        {
            this.scopes.Push(scope);
        }

        public virtual FlowElementScope PeekScope() => this.scopes.Peek();

        public virtual FlowElementScope PopScope() => this.scopes.Pop();
    }
}
