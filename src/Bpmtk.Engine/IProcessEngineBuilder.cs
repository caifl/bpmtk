
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Bpmtk.Infrastructure;

namespace Bpmtk.Engine
{
    public interface IProcessEngineBuilder
    {
        IServiceCollection Services
        {
            get;
        }

        IProcessEngineBuilder ConfigureServices(Action<IServiceCollection> configureAction);

        IProcessEngineBuilder AddUnitOfWork(Func<IServiceProvider, IUnitOfWork> buildAction);

        IProcessEngineBuilder AddUnitOfWork<TUnitOfWork>()
            where TUnitOfWork : IUnitOfWork;

        IProcessEngineBuilder AddDeploymentStore<TDeploymentStore>();

        IProcessEngineBuilder UseApplicationServices(IServiceProvider serviceProvider);

        IProcessEngine Build();
    }
}
