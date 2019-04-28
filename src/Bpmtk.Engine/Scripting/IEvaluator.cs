using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Scripting
{
    public interface IEvaluator
    {
        object Evalute(string script);

        string EvaluteToString(string text);
    }
}
