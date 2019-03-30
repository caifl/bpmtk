using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class PropertyParseHandler : BaseElementParseHandler
    {
        public PropertyParseHandler()
        {
            this.handlers.Add("dataState", new DataStateParseHandler());
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var prop = context.BpmnFactory.CreateProperty();

            prop.Name = element.GetAttribute("name");
            //prop.ItemSubjectRef = element.GetAttribute("itemSubjectRef");
            prop.IsCollection = element.GetBoolean("isCollection");

            //parent.Properties.Add(prop);

            return prop;
        }
    }

    class DataStateParseHandler : BaseElementParseHandler
    {
        public override object Create(object parent, IParseContext context, XElement element)
        {
            var dataState = context.BpmnFactory.CreateDataState();
            dataState.Name = element.GetAttribute("name");

            return dataState;
        }
    }
}
