using System;
using System.Collections.Generic;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IActivityInstanceQuery
    {
        IList<ActivityInstance> List();
    }
}
