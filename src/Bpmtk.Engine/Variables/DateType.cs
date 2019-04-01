using System;

namespace Bpmtk.Engine.Variables
{
    public class DateType : VariableType
    {
        public override string Name => "date";

        public override object GetValue(IValueFields fields)
        {
            var ticks = fields.LongValue;
            if (ticks != null)
            {
                return new DateTime(ticks.Value);
            }

            return null;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || value is DateTime;
        }

        public override void SetValue(IValueFields fields, object value)
        {
            if(value == null)
            {
                fields.LongValue = null;
                return;
            }

            fields.LongValue = Convert.ToDateTime(value).Ticks;
        }
    }
}
