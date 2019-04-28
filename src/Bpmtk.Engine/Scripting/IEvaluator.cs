using System;

namespace Bpmtk.Engine.Scripting
{
    public interface IEvaluator
    {
        object Evalute(string script);

        TValue Evalute<TValue>(string script);

        string EvaluteToString(string text);
    }
}
