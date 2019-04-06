using System;

namespace Bpmtk.Engine.Scripting
{
    public interface IVariableResolver
    {
        bool Resolve(string name, out object value);
    }
}
