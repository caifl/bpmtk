using System;

namespace Bpmtk.Engine.Variables
{
    public class ByteArrayType : VariableType
    {
        public override string Name => "bytes";

        public override object GetValue(IValueFields fields)
        {
            return fields.Bytes;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || value is byte[];
        }

        public override void SetValue(IValueFields fields, object value)
        {
            fields.Bytes = (byte[])value;
        }
    }
}
