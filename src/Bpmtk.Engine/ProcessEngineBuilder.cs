using System;
using System.Collections.Generic;
using Bpmtk.Engine.Identity;
using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Storage;
using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine
{
    public class ProcessEngineBuilder : IProcessEngineBuilder
    {
        //private readonly IServiceCollection services;
        //private readonly bool useExternalServices;
        //private readonly List<Action<IServiceCollection>> configureActions = new List<Action<IServiceCollection>>();

        public ProcessEngineBuilder()
        {
            //this.services = new ServiceCollection();
        }

        //public ProcessEngineBuilder(IServiceCollection services)
        //{
        //    this.services = services;
        //    this.useExternalServices = true;

        //    this.AddCoreServices();
        //}

        //protected virtual void AddStores()
        //{
        //    this.services.AddTransient<ITaskStore, TaskStore>();
        //    this.services.AddTransient<IDeploymentStore, DeploymentStore>();
        //    this.services.AddTransient<IProcessInstanceStore, ProcessInstanceStore>();
        //}

        //protected virtual void AddCoreServices()
        //{
        //    //add core services.
        //    this.services.AddTransient<ITaskManager, TaskManager>();
        //    this.services.AddTransient<IDeploymentManager, DeploymentManager>();
        //    this.services.AddTransient<IRuntimeManager, RuntimeManager>();
        //    this.services.AddTransient<IIdentityManager, IdentityManager>();
        //}

        //protected virtual void AddInternalServices()
        //{
        //    //add core services.
        //    this.services.AddTransient<IDeploymentManager, DeploymentManager>();
        //    this.services.AddTransient<IHumanTaskHandler, HumanTaskHandler>();
        //    //this.services.AddTransient<IRuntimeService, ExecutionService>();
        //}

        //public IProcessEngineBuilder AddUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
        //{
        //    this.services.AddScoped(typeof(IUnitOfWork), typeof(TUnitOfWork));

        //    return this;
        //}

        //public IProcessEngineBuilder AddUnitOfWork(Func<IServiceProvider, IUnitOfWork> factory)
        //{
        //    this.services.AddScoped<IUnitOfWork>((sp) => factory(sp));

        //    return this;
        //}

        public virtual IProcessEngine Build()
        {
            //foreach (var action in this.configureActions)
            //    action(this.services);

            //this.AddInternalServices();
            //this.AddCoreServices();

            //var engineServices = this.useExternalServices ? this.serviceProvider : this.services.BuildServiceProvider();
            return new ProcessEngine(this);
        }

        protected IDbSessionFactory dbSessionFactory;
        protected ILoggerFactory loggerFactory;

        public virtual IDbSessionFactory DbSessionFactory
        {
            get => this.dbSessionFactory;
        }

        public virtual ILoggerFactory LoggerFactory
        {
            get => this.loggerFactory;
        }

        public virtual IProcessEngineBuilder SetDbSessionFactory(IDbSessionFactory dbSessionFactory)
        {
            this.dbSessionFactory = dbSessionFactory;

            return this;
        }

        public virtual IProcessEngineBuilder SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;

            return this;
        }

        //public virtual IProcessEngineBuilder ConfigureServices(Action<IServiceCollection> configureAction)
        //{
        //    this.configureActions.Add(configureAction);

        //    return this;
        //}

        //    public virtual IServiceCollection Services
        //    {
        //        get => this.services;
        //    }

        //    private IServiceProvider serviceProvider;

        //    public IProcessEngineBuilder UseApplicationServices(IServiceProvider serviceProvider)
        //    {
        //        this.serviceProvider = serviceProvider;

        //        return this;
        //    }
        //}
    }
}
