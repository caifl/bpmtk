using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public interface IContextFactory
    {
        IContext Create(IProcessEngine engine);
    }
}
