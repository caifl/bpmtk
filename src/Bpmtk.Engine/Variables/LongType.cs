using System;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Variables
{
    public class LongType : VariableType
    {
        public override string Name => "long";

        public override object GetValue(IValueFields fields)
        {
            return fields.LongValue;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || TypeHelper.IsInteger(value);
        }

        public override void SetValue(IValueFields fields, object value)
        {
            if (value != null)
                fields.LongValue = Convert.ToInt64(value);
            else
                fields.LongValue = null;           
        }
    }
}
