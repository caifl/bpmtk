using Bpmtk.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Variables
{
    /// <summary>
    /// System.Int32 type.
    /// </summary>
    public class IntegerType : VariableType
    {
        public override string Name => "int";

        public override object GetValue(IValueFields fields)
        {
            var value = fields.LongValue;
            if (value != null)
                return Convert.ToInt32(value);

            return null;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || TypeHelper.IsInteger(value);
        }

        public override void SetValue(IValueFields fields, object value)
        {
            if (value != null)
                fields.LongValue = Convert.ToInt32(value);
            else
                fields.LongValue = null;
        }
    }
}
