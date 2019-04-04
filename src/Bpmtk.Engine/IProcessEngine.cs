using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    public interface IProcessEngine
    {
        IContext CreateContext();

        IContext CreateContext(IServiceProvider services);
    }
}
