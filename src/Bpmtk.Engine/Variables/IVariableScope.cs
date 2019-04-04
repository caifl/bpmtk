using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Variables
{
    public interface IVariableScope
    {
        object GetVariable(string name);
    }
}
