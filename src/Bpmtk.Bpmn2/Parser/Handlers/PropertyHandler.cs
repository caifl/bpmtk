using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class PropertyHandler<TPropertyContainer> : BaseElementHandler<TPropertyContainer, Property>
        where TPropertyContainer : IPropertyContainer
    {
        public PropertyHandler()
        {
            this.handlers.Add("dataState", new DataStateHandler<Property>());
        }

        public override Property Create(TPropertyContainer parent, IParseContext context, XElement element)
        {
            var prop = base.Create(parent, context, element);

            prop.Name = element.GetAttribute("name");
            prop.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            prop.IsCollection = element.GetBoolean("isCollection");

            parent.Properties.Add(prop);

            return prop;
        }

        protected override Property New(IParseContext context, XElement element) => context.BpmnFactory.CreateProperty();
    }

    class DataStateHandler<TItemAwareElement> : BaseElementHandler<TItemAwareElement, DataState>
        where TItemAwareElement : IItemAwareElement
    {
        public override DataState Create(TItemAwareElement parent, IParseContext context, XElement element)
        {
            var dataState = base.Create(parent, context, element);
            dataState.Name = element.GetAttribute("name");

            return parent.DataState = dataState;
        }

        protected override DataState New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataState();
    }
}
