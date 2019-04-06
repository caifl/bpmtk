using System;

namespace Bpmtk.Engine.Bpmn2.Types
{
    public interface ITypeHandler
    {
        string Name
        {
            get;
        }

        Type ClrType
        {
            get;
        }

        object Parse(string[] values, bool isCollection = false);
    }
}
