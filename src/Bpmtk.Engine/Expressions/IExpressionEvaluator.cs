using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Expressions
{
    public interface IExpressionEvaluator
    {
        TResult Evaluate<TResult>(IEvaluationContext context, 
            string expression);
    }
}
