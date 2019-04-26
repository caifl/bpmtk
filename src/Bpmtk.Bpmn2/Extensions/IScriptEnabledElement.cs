using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2.Extensions
{
    public interface IScriptEnabledElement
    {
        IList<Script> Scripts
        {
            get;
        }
    }
}
