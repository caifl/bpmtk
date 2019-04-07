using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    public interface IActivityInstanceQuery
    {
        IList<ActivityInstance> List();
    }
}
