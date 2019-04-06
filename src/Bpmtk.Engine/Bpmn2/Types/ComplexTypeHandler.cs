using System;

namespace Bpmtk.Engine.Bpmn2.Types
{
    public class ComplexTypeHandler : ITypeHandler
    {
        public ComplexTypeHandler(string typeName)
        {
            this.Name = typeName;
        }

        public string Name
        {
            get;
        }

        public Type ClrType => null;

        public object Parse(string[] values, bool isCollection = false)
        {
            throw new NotImplementedException();
        }
    }
}
