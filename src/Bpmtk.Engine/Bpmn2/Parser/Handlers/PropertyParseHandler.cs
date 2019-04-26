using System;
using System.Xml.Linq;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    class PropertyParseHandler<TParent> : BaseElementParseHandler<TParent>
    {
        private readonly Action<TParent, Property> callback;

        public PropertyParseHandler(Action<TParent, Property> callback)
        {
            this.handlers.Add("dataState", new DataStateParseHandler());
            this.callback = callback;
        }

        public override object Create(TParent parent, IParseContext context, XElement element)
        {
            var props = context.BpmnFactory.CreateProperty();

            props.Name = element.GetAttribute("name");

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if (itemSubjectRef != null)
                context.AddReferenceRequest<ItemDefinition>(itemSubjectRef, x => props.ItemSubjectRef = x);

            if (this.callback != null)
                this.callback(parent, props);

            base.Init(props, context, element);

            context.Push(props);

            return props;
        }
    }

    class DataStateParseHandler : BaseElementParseHandler<IItemAwareElement>
    {
        public override object Create(IItemAwareElement parent, IParseContext context, XElement element)
        {
            var dataState = context.BpmnFactory.CreateDataState();
            parent.DataState = dataState;

            dataState.Name = element.GetAttribute("name");

            base.Init(dataState, context, element);

            return dataState;
        }
    }
}
