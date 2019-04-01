using System;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Variables
{
    public class DoubleType : VariableType
    {
        public override string Name => "double";

        public override object GetValue(IValueFields fields)
        {
            return fields.DoubleValue;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || TypeHelper.IsDouble(value);
        }

        public override void SetValue(IValueFields fields, object value)
        {
            fields.DoubleValue = (double?)value;
        }
    }
}
