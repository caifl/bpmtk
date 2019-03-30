using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2.Parser
{
    class ResourceRoleParseHandler : BaseElementParseHandler
    {
        public static readonly string[] Keys = new string[]
        {
            "potentialOwner",
            "humanPerformer",
            "performer",
            "resourceRole"
        };

        public ResourceRoleParseHandler()
        {
            this.handlers.Add("resourceRef", new ParseHandlerAction<ResourceRole>((parent, context, element) =>
            {
                var resourceRef = element.Value;
                if (resourceRef != null)
                    context.AddReferenceRequest<Resource>(resourceRef, (r) => parent.ResourceRef = r);
            }));

            this.handlers.Add("resourceAssignmentExpression", new ResourceAssignmentExpressionParseHandler());
            this.handlers.Add("resourceParameterBinding", new ResourceParameterBindingParseHandler());
        }

        public override object Create(object parent, IParseContext context, XElement element)
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

            resourceRole.Name = element.GetAttribute("name");

            var value = element.GetAttribute("type");
            if (value != null)
                resourceRole.Type = (ResourceType)Enum.Parse(typeof(ResourceType), value);

            ICollection<ResourceRole> list = null;
            if (parent is Process)
                list = (parent as Process).ResourceRoles;
            else if (parent is Activity)
                list = (parent as Activity).ResourceRoles;

            if (list != null)
                list.Add(resourceRole);

            if (element.HasElements)
                this.CreateChildren(resourceRole, context, element);

            return resourceRole;
        }
    }

    class ResourceParameterBindingParseHandler : BaseElementParseHandler<ResourceRole>
    {
        public ResourceParameterBindingParseHandler()
        {
            this.handlers.Add("expression", new ExpressionParseHandler<ResourceParameterBinding>((parent, expression) =>
            {
                parent.Expression = expression;
            }));

            this.handlers.Add("formalExpression", new ExpressionParseHandler<ResourceParameterBinding>((parent, expression) =>
            {
                parent.Expression = expression;
            }));
        }

        public override object Create(ResourceRole parent, IParseContext context, XElement element)
        {
            var binding = context.BpmnFactory.CreateResourceParameterBinding();

            var parameterRef = element.GetAttribute("parameterRef");

            //binding.ParameterRef

            parent.ParameterBindings.Add(binding);

            return binding;
        }
    }

    class ResourceAssignmentExpressionParseHandler : BaseElementParseHandler<ResourceRole>
    {
        public ResourceAssignmentExpressionParseHandler()
        {
            this.handlers.Add("expression", new ExpressionParseHandler<ResourceAssignmentExpression>((parent, expression) =>
            {
                parent.Expression = expression;
            }));

            this.handlers.Add("formalExpression", new ExpressionParseHandler<ResourceAssignmentExpression>((parent, expression) =>
            {
                parent.Expression = expression;
            }));
        }

        public override object Create(ResourceRole parent, IParseContext context, XElement element)
        {
            var expr = context.BpmnFactory.CreateResourceAssignmentExpression();
            parent.AssignmentExpression = expr;

            base.Init(expr, context, element);

            return expr;
        }
    }
}
