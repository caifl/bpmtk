using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    class DataObjectParseHandler : FlowElementParseHandler
    {
        public DataObjectParseHandler()
        {
            //this.handlers.Add("dataState", new DataStateHandler<DataObject>());
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var dataObject = context.BpmnFactory.CreateDataObject();
            parent.FlowElements.Add(dataObject);

            //dataObject.Name = element.GetAttribute("name");
            dataObject.IsCollection = element.GetBoolean("isCollection");

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, x => dataObject.ItemSubjectRef = x);

            base.Init(dataObject, context, element);

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
            var dataObjectReference = context.BpmnFactory.CreateDataObjectReference();
            parent.FlowElements.Add(dataObjectReference);

            //dataObjectReference.Name = element.GetAttribute("name");

            var dataObjectRef = element.GetAttribute("dataObjectRef");
            if (dataObjectRef != null)
                context.AddReferenceRequest<DataObject>(dataObjectRef, x => dataObjectReference.DataObjectRef = x);

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, x => dataObjectReference.ItemSubjectRef = x);

            base.Init(dataObjectReference, context, element);

            return dataObjectRef;
        }
    }
}
