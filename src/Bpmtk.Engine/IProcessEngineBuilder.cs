
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine
{
    public interface IProcessEngineBuilder
    {
        IProcessEngineBuilder SetContextFactory(IContextFactory contextFactory);

        IProcessEngineBuilder SetLoggerFactory(ILoggerFactory loggerFactory);

        IProcessEngine Build();
    }
}
