using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Internal
{
    public static class TKContext
    {
        public static TService GetService<TService>()
        {
            return default(TService);
        }
    }
}
