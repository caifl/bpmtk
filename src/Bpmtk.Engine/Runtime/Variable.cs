using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    /// <summary>
    /// Runtime Variable Object.
    /// </summary>
    public class Variable : VariableInstance
    {
        public Variable(string name, object value) : base(name, value)
        {
        }
    }
}
