using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class DataObjectParseHandler : FlowElementParseHandler
    {
        public DataObjectParseHandler()
        {
            //this.handlers.Add("dataState", new DataStateHandler<DataObject>());
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var dataObject = context.BpmnFactory.CreateDataObject() as ValuedDataObject;
            parent.FlowElements.Add(dataObject);

            //dataObject.Name = element.GetAttribute("name");
            dataObject.IsCollection = element.GetBoolean("isCollection");
            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            dataObject.TypeName = itemSubjectRef;
            
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, x => dataObject.ItemSubjectRef = x);

            base.Init(dataObject, context, element);

            return dataObject;
        }

        protected override ExtensionElements ParseExtensionElements(BaseElement parent, IParseContext context, XElement element)
        {
            var dataObject = parent as ValuedDataObject;
            var item = context.BpmnFactory.CreateExtensionElements();

            var children = element.Elements();
            foreach(var child in children)
            {
                var localName = child.Name.LocalName;
                if(localName == "value")
                {
                    dataObject.Values.Add(child.Value);
                }
            }

            return item;//base.ParseExtensionElements(parent, context, element);
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
