using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Bpmn2.Extensions
{
    public interface IScriptEnabledElement
    {
        IList<Script> Scripts
        {
            get;
        }
    }
}
