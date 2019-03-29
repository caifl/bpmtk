using System;
using System.Xml.Linq;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2.Parser
{
    class ResourceRoleHandler<TResourceRoleContainer> : BaseElementHandler<TResourceRoleContainer, ResourceRole>
        where TResourceRoleContainer : IResourceRoleContainer
    {
        public static readonly string[] Keys = new string[]
        {
            "potentialOwner",
            "humanPerformer",
            "performer",
            "resourceRole"
        };

        public ResourceRoleHandler()
        {
            this.handlers.Add("resourceRef", new BpmnHandlerCallback<ResourceRole>((parent, context, element) =>
            {
                return parent.ResourceRef = element.Value;
            }));

            this.handlers.Add("resourceAssignmentExpression", new ResourceAssignmentExpressionHandler());
            this.handlers.Add("resourceParameterBinding", new ResourceParameterBindingHandler());
        }

        public override ResourceRole Create(TResourceRoleContainer parent, IParseContext context, XElement element)
        {
            var resourceRole = base.Create(parent, context, element);

            resourceRole.Name = element.GetAttribute("name");

            var value = element.GetAttribute("type");
            if (value != null)
                resourceRole.Type = (ResourceType)Enum.Parse(typeof(ResourceType), value);


            parent.ResourceRoles.Add(resourceRole);

            return resourceRole;
        }

        protected override ResourceRole New(IParseContext context, XElement element)
        {
            ResourceRole resourceRole = null;
            var factory = context.BpmnFactory;
            var type = Helper.GetRealLocalName(element);

            switch (type)
            {
                case "potentialOwner":
                    resourceRole = factory.CreatePotentialOwner();
                    break;

                case "humanPerformer":
                    resourceRole = factory.CreateHumanPerformer();
                    break;

                case "performer":
                    resourceRole = factory.CreatePerformer();
                    break;

                case "resourceRole":
                    resourceRole = factory.CreateResourceRole();
                    break;
            }

            return resourceRole;
        }
    }

    class ResourceParameterBindingHandler : BaseElementHandler<ResourceRole, ResourceParameterBinding>
    {
        public ResourceParameterBindingHandler()
        {
            this.handlers.Add("expression", new ExpressionHandler<ResourceParameterBinding>((parent, context, element, expression) =>
            {
                parent.Expression = expression;
            }));

            this.handlers.Add("formalExpression", new ExpressionHandler<ResourceParameterBinding>((parent, context, element, expression) =>
            {
                parent.Expression = expression;
            }));
        }

        public override ResourceParameterBinding Create(ResourceRole parent, IParseContext context, XElement element)
        {
            var binding = base.Create(parent, context, element);

            binding.ParameterRef = element.GetAttribute("parameterRef");

            parent.ParameterBindings.Add(binding);

            return binding;
        }

        protected override ResourceParameterBinding New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateResourceParameterBinding();
    }

    class ResourceAssignmentExpressionHandler : BaseElementHandler<ResourceRole, ResourceAssignmentExpression>
    {
        public ResourceAssignmentExpressionHandler()
        {
            this.handlers.Add("expression", new ExpressionHandler<ResourceAssignmentExpression>((parent, context, element, expression) =>
            {
                parent.Expression = expression;
            }));

            this.handlers.Add("formalExpression", new ExpressionHandler<ResourceAssignmentExpression>((parent, context, element, expression) =>
            {
                parent.Expression = expression;
            }));
        }

        public override ResourceAssignmentExpression Create(ResourceRole parent, IParseContext context, XElement element)
        {
            return parent.AssignmentExpression = base.Create(parent, context, element);
        }

        protected override ResourceAssignmentExpression New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateResourceAssignmentExpression();
    }
}
