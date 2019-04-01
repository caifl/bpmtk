using System;

namespace Bpmtk.Engine.Variables
{
    public class GuidType : VariableType
    {
        public override string Name => "guid";

        public override object GetValue(IValueFields fields)
        {
            if (fields.CachedValue != null)
                return fields.CachedValue;

            var value = fields.Text;
            if (value != null)
                return Guid.Parse(value);

            return null;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null || value is Guid;
        }

        public override void SetValue(IValueFields fields, object value)
        {
            fields.CachedValue = value;

            if (value != null)
                fields.Text = value.ToString();
        }
    }
}
