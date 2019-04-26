
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IProcessEngineBuilder
    {
        IProcessEngineBuilder SetDbSessionFactory(IDbSessionFactory dbSessionFactory);

        IProcessEngineBuilder SetLoggerFactory(ILoggerFactory loggerFactory);

        IProcessEngine Build();
    }
}
