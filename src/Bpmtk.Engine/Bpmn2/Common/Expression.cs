using System;
using Bpmtk.Engine.Expressions;

namespace Bpmtk.Engine.Bpmn2
{
    public class Expression : BaseElementWithMixedContent,
         IExpression
    {
        //private IExpression compiledExpression;

        public virtual object GetValue(IEvaluationContext context)
        {
            throw new NotImplementedException();
        }

        public virtual TValue GetValue<TValue>(IEvaluationContext context)
        {
            var value = this.GetValue<TValue>(context);
            if (value != null)
                return (TValue)value;

            return default(TValue);
        }
    }

    public class FormalExpression : Expression
    {
        public virtual string Language
        {
            get;
            set;
        }

        public virtual string EvaluatesToTypeRef
        {
            get;
            set;
        }
    }
}
