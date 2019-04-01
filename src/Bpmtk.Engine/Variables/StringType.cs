using System;

namespace Bpmtk.Engine.Variables
{
    public class StringType : VariableType
    {
        public override string Name => "string";

        public override object GetValue(IValueFields fields)
        {
            return fields.Text;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || value is string || typeof(string).IsAssignableFrom(value.GetType());
        }

        public override void SetValue(IValueFields fields, object value)
        {
            fields.Text = value?.ToString();
        }
    }
}
