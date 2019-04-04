using System;

namespace Bpmtk.Engine.Expressions
{
    public interface IExpression
    {
        object GetValue(IEvaluationContext context);

        TValue GetValue<TValue>(IEvaluationContext context);
    }
}
