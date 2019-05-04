using System;

namespace Bpmtk.Engine.Scripting
{
    public interface IEvaluator
    {
        object Evaluate(string script);

        TValue Evaluate<TValue>(string script);

        string EvaluteToString(string text);
    }
}
