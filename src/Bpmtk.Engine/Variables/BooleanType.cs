using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Variables
{
    public class BooleanType : VariableType
    {
        public override string Name => "bool";

        public override object GetValue(IValueFields fields)
        {
            if (fields.LongValue != null)
            {
                return fields.LongValue == 1;
            }

            return null;
        }

        public override bool IsAssignableFrom(object value)
        {
            if (value == null)
                return true;

            return typeof(Boolean).IsAssignableFrom(value.GetType());
        }

        public override void SetValue(IValueFields fields, object value)
        {
            if (value == null)
            {
                fields.LongValue = null;
            }
            else
            {
                Boolean booleanValue = (Boolean)value;
                if (booleanValue)
                {
                    fields.LongValue = 1L;
                }
                else
                {
                    fields.LongValue = 0L;
                }
            }
        }
    }
}
