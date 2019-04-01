using System;
using System.Collections.Generic;
using System.Text;

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

        IVariableType Type
        {
            get;
        }
    }
}
