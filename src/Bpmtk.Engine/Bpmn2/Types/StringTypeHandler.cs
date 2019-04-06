using System;

namespace Bpmtk.Engine.Bpmn2.Types
{
    public class StringTypeHandler : ITypeHandler
    {
        public StringTypeHandler()
        {
            this.Name = "string";
            this.ClrType = typeof(string);
        }

        public virtual string Name
        {
            get;
        }

        public virtual Type ClrType
        {
            get;
        }

        public virtual object Parse(string[] array, bool isCollection = false)
        {
            if (array == null || array.Length == 0)
                return null;

            if (!isCollection)
                return array[0];

            return array;
        }
    }
}
