using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Variables
{
    public interface IVariableType
    {
        string Name
        {
            get;
        }

        bool IsCachable
        {
            get;
        }

        bool IsAssignableFrom(object value);

        void SetValue(IValueFields fields, object value);

        object GetValue(IValueFields fields);
    }
}
