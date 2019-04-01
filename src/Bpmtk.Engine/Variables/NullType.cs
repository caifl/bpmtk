using System;

namespace Bpmtk.Engine.Variables
{
    public class NullType : VariableType
    {
        public override string Name => "null";

        public override object GetValue(IValueFields fields)
        {
            return null;
        }

        public override bool IsAssignableFrom(object value)
        {
            return value is null;
        }

        public override void SetValue(IValueFields fields, object value)
        {
            
        }
    }
}
