using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ResourceParseHandler : BaseElementParseHandler
    {
        public ResourceParseHandler()
        {
            handlers.Add("resourceParameter", new ResourceParameterParseHandler());
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var resource = context.BpmnFactory.CreateResource();

            resource.Name = element.GetAttribute("name");

            ((Definitions)parent).RootElements.Add(resource);

            return resource;
        }
    }

    class ResourceParameterParseHandler : BaseElementParseHandler
    {
        public override object Create(object parent, IParseContext context, XElement element)
        {
            var parameter = context.BpmnFactory.CreateResourceParameter();

            parameter.Name = element.GetAttribute("name");
            //parameter.Type = element.GetAttribute("type");

            context.AddReferenceRequest(element.GetAttribute("type"), delegate (ItemDefinition target)
            {
                parameter.Type = target;
            });

            var v = element.GetAttribute("isRequired");
            if (v != null)
                parameter.IsRequired = bool.Parse(v);

            //parameter.Value = element.GetExtendedAttribute("value");

            ((Resource)parent).Parameters.Add(parameter);

            return parameter;
        }
    }
}
