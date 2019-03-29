using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ResourceHandler : BaseElementHandler<Definitions, Resource>
    {
        public ResourceHandler()
        {
            handlers.Add("resourceParameter", new ResourceParameterHandler());
        }

        public override Resource Create(Definitions parent, IParseContext context, XElement element)
        {
            var resource = base.Create(parent, context, element);

            resource.Name = element.GetAttribute("name");

            parent.RootElements.Add(resource);

            return resource;
        }

        protected override Resource New(IParseContext context, XElement element) => context.BpmnFactory.CreateResource();
    }

    class ResourceParameterHandler : BaseElementHandler<Resource, ResourceParameter>
    {
        public override ResourceParameter Create(Resource parent, IParseContext context, XElement element)
        {
            var parameter = base.Create(parent, context, element);

            parameter.Name = element.GetAttribute("name");
            parameter.Type = element.GetAttribute("type");

            var v = element.GetAttribute("isRequired");
            if (v != null)
                parameter.IsRequired = bool.Parse(v);

            //parameter.Value = element.GetExtendedAttribute("value");

            parent.Parameters.Add(parameter);

            return parameter;
        }

        protected override ResourceParameter New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateResourceParameter();
    }
}
