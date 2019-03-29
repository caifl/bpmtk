using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ExpressionHandler<TParent> : IBpmnHandler<TParent>
    {
        protected readonly Action<TParent, IParseContext, XElement, Expression> callback;

        public ExpressionHandler(Action<TParent, IParseContext, XElement, Expression> callback)
        {
            this.callback = callback;
        }

        public virtual object Create(TParent parent, IParseContext context, XElement element)
        {
            Expression expression = null;
            if (Helper.IsFormalExpression(element))
            {
                var formalExpression = context.BpmnFactory.CreateFormalExpression();

                formalExpression.EvaluatesToTypeRef = element.GetAttribute("evaluatesToTypeRef");
                formalExpression.Language = element.GetAttribute("language");

                expression = formalExpression;
            }
            else
                expression = context.BpmnFactory.CreateExpression();

            expression.Id = element.GetAttribute("id");
            expression.Text = element.Value;
            expression.Documentation = element.GetElement("documentation")?.Value;

            this.callback(parent, context, element, expression);

            return expression;
        }
    }
}
