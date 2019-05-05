using System;

namespace Bpmtk.Engine.Variables
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
