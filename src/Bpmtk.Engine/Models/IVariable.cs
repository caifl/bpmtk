using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Models
{
    public interface IVariable
    {
        long Id
        {
            get;
        }

        string Name
        {
            get;
        }

        object GetValue();

        void SetValue(object value);
    }
}
