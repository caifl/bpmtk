using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class FlowElementHandler<TFlowElementContainer, TFlowElement>
        : BaseElementHandler<TFlowElementContainer, TFlowElement>
        where TFlowElement : FlowElement
        where TFlowElementContainer : IFlowElementsContainer
    {
        public override TFlowElement Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var flowElement = base.Create(parent, context, element);

            flowElement.Name = element.GetAttribute("name");
            flowElement.Container = parent;

            parent.FlowElements.Add(flowElement);

            var scope = context.PeekScope();
            scope.Add(flowElement);

            return flowElement;
        }
    }

    abstract class FlowNodeHandler<TFlowElementContainer, TFlowNode> : FlowElementHandler<TFlowElementContainer, TFlowNode>
        where TFlowNode : FlowNode
        where TFlowElementContainer : IFlowElementsContainer
    {
        public FlowNodeHandler()
        {
            this.handlers.Add("incoming", new BpmnHandlerCallback<TFlowNode>((parent, context, element) =>
            {
                var incoming = element.Value;

                var scope = context.PeekScope();
                scope.AddIncoming(incoming, parent);

                return incoming;
            }));

            this.handlers.Add("outgoing", new BpmnHandlerCallback<TFlowNode>((parent, context, element) =>
            {
                var outgoing = element.Value;

                var scope = context.PeekScope();
                scope.AddOutgoing(outgoing, parent);

                return outgoing;
            }));
        }

        public override TFlowNode Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var flowNode = base.Create(parent, context, element);

            if (flowNode.ExtensionElements != null)
            {
                var items = this.ParseEventListeners(flowNode.ExtensionElements);
                foreach (var item in items)
                    flowNode.EventListeners.Add(item);
            }

            return flowNode;
        }
    }

    class DataObjectHandler<TFlowElementContainer> : FlowElementHandler<TFlowElementContainer, DataObject>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public DataObjectHandler()
        {
            this.handlers.Add("dataState", new DataStateHandler<DataObject>());
        }

        public override DataObject Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var dataObject = base.Create(parent, context, element);

            dataObject.Name = element.GetAttribute("name");
            dataObject.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            dataObject.IsCollection = element.GetBoolean("isCollection");

            return dataObject;
        }

        protected override DataObject New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataObject();
    }

    class DataObjectReferenceHandler<TFlowElementContainer> : FlowElementHandler<TFlowElementContainer, DataObjectReference>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public DataObjectReferenceHandler()
        {
            //this.handlers.Add("dataState", new DataStateHandler<DataObjectReference>());
        }

        public override DataObjectReference Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var dataObject = base.Create(parent, context, element);

            dataObject.Name = element.GetAttribute("name");
            dataObject.ItemSubjectRef = element.GetAttribute("itemSubjectRef");

            return dataObject;
        }

        protected override DataObjectReference New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataObjectReference();
    }
}
