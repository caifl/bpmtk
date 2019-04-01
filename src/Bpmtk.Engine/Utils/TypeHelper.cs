using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine.Utils
{
    internal static class TypeHelper
    {
        public static bool IsInteger(object value)
        {
            return value is sbyte
                        || value is byte
                        || value is short
                        || value is ushort
                        || value is int
                        || value is uint
                        || value is long;
        }

        public static bool IsDouble(object value)
        {
            return IsInteger(value) || value is ulong || value is float || value is double;
        }

        public static bool IsSerialiable(object value)
        {
            if (value is ISerializable)
                return true;

            return Attribute.IsDefined(value.GetType(), typeof(SerializableAttribute));
        }
    }
}
