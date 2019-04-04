using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Scripting
{
    public interface IScriptEngine
    {
        object Execute(string script);
    }
}
