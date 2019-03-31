using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ResourceParseHandler : BaseElementParseHandler<Definitions>
    {
        public ResourceParseHandler()
        {
            handlers.Add("resourceParameter", new ResourceParameterParseHandler());
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var resource = context.BpmnFactory.CreateResource();
            parent.RootElements.Add(resource);

            resource.Name = element.GetAttribute("name");

            base.Init(resource, context, element);

            context.Push(resource);

            return resource;
        }
    }

    class ResourceParameterParseHandler : BaseElementParseHandler<Resource>
    {
        public override object Create(Resource parent, IParseContext context, XElement element)
        {
            var parameter = context.BpmnFactory.CreateResourceParameter();
            parent.Parameters.Add(parameter);

            parameter.Name = element.GetAttribute("name");

            var type = element.GetAttribute("type");
            if(type != null)
            context.AddReferenceRequest<ItemDefinition>(type, r => parameter.Type = r);

            var v = element.GetAttribute("isRequired");
            if (v != null)
                parameter.IsRequired = bool.Parse(v);

            //parameter.Value = element.GetExtendedAttribute("value");
            base.Init(parameter, context, element);

            context.Push(parameter);

            return parameter;
        }
    }
}
