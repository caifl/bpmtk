using System;

namespace Bpmtk.Engine.Scripting
{
    public interface IScriptingScope : IDisposable
    {
        IScriptingScope SetVariableResolver(IVariableResolver variableResolver);

        IScriptingScope SetValue(string name, object value);

        IScriptingScope SetValue(string name, Delegate value);

        object GetValue(string name);

        IScriptingScope AddClrType(string name, Type type);

        IScriptingScope AddClrType(Type type);
    }
}
