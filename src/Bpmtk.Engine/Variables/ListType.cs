using System;
using System.Collections;

namespace Bpmtk.Engine.Variables
{
    public class ListType : SerializableType
    {
        public override string Name => "list";

        public override bool IsAssignableFrom(object value)
        {
            return value is IList;
        }
    }
}
