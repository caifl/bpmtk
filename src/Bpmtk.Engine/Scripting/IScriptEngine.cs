using System.Collections.Generic;

namespace Bpmtk.Engine.Scripting
{
    public interface IScriptEngine
    {
        string Name
        {
            get;
        }

        object CreateCompileUnit(string script);

        object CreateCompileUnit(string script,
            out bool isExpression);

        IScriptingScope CreateScope(IVariableResolver variableResolver = null);

        object Execute(string script, IScriptingScope scope);

        object ExecuteCompileUnit(object compileUnit, IScriptingScope scope);
    }
}
