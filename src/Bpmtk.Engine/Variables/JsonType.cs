using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Variables
{
    public class JsonType : VariableType
    {
        public override string Name => "json";

        public override object GetValue(IValueFields fields)
        {
            throw new NotImplementedException();
        }

        public override bool IsAssignableFrom(object value)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(IValueFields fields, object value)
        {
            throw new NotImplementedException();
        }
    }
}
