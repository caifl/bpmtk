using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    abstract class FlowElementParseHandler : BaseElementParseHandler<IFlowElementsContainer>
    {
        protected virtual void Init(FlowElement flowElement, IParseContext context, XElement element)
        {
            flowElement.Name = element.GetAttribute("name");
        }
    }

    abstract class FlowNodeParseHandler : FlowElementParseHandler
    {
        public FlowNodeParseHandler()
        {
            this.handlers.Add("incoming", new ParseHandlerAction<FlowNode>((parent, context, element) =>
            {
                var incoming = element.Value;
                if (incoming != null)
                    context.AddReferenceRequest<SequenceFlow>(incoming, (sf) => parent.Incomings.Add(sf));
            }));

            this.handlers.Add("outgoing", new ParseHandlerAction<FlowNode>((parent, context, element) =>
            {
                var outgoing = element.Value;
                if (outgoing != null)
                    context.AddReferenceRequest<SequenceFlow>(outgoing, (sf) => parent.Outgoings.Add(sf));
            }));
        }

        //protected virtual void Parse(FlowNode flowNode, IParseContext context, XElement element)
        //{
        //    base.Parse(flowNode, context, element);
        //    //var flowNode = base.Create(parent, context, element);

        //    //if (flowNode.ExtensionElements != null)
        //    //{
        //    //    var items = this.ParseEventListeners(flowNode.ExtensionElements);
        //    //    foreach (var item in items)
        //    //        flowNode.EventListeners.Add(item);
        //    //}

        //    //return flowNode;
        //}
    }

    class DataObjectParseHandler : FlowElementParseHandler
    {
        public DataObjectParseHandler()
        {
            //this.handlers.Add("dataState", new DataStateHandler<DataObject>());
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var dataObject = context.BpmnFactory.CreateDataObject();

            dataObject.Name = element.GetAttribute("name");
            //dataObject.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            dataObject.IsCollection = element.GetBoolean("isCollection");

            return dataObject;
        }
    }

    class DataObjectReferenceParseHandler : FlowElementParseHandler
    {
        public DataObjectReferenceParseHandler()
        {
            //this.handlers.Add("dataState", new DataStateHandler<DataObjectReference>());
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var dataObjectRef = context.BpmnFactory.CreateDataObjectReference();

            dataObjectRef.Name = element.GetAttribute("name");
            //dataObject.ItemSubjectRef = element.GetAttribute("itemSubjectRef");

            return dataObjectRef;
        }
    }
}
