using System;
using System.Collections.Generic;
using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Repository.Internal;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Stores.Internal;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Tasks.Internal;
using Bpmtk.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Bpmtk.Engine
{
    public class ProcessEngineBuilder : IProcessEngineBuilder
    {
        private readonly IServiceCollection services;
        private readonly bool useExternalServices;
        private readonly List<Action<IServiceCollection>> configureActions = new List<Action<IServiceCollection>>();

        public ProcessEngineBuilder()
        {
            this.services = new ServiceCollection();
        }

        public ProcessEngineBuilder(IServiceCollection services)
        {
            this.services = services;
            this.useExternalServices = true;

            this.AddCoreServices();
        }

        //protected virtual void AddStores()
        //{
        //    this.services.AddTransient<ITaskStore, TaskStore>();
        //    this.services.AddTransient<IDeploymentStore, DeploymentStore>();
        //    this.services.AddTransient<IProcessInstanceStore, ProcessInstanceStore>();
        //}

        protected virtual void AddCoreServices()
        {
            //add core services.
            this.services.AddTransient<ITaskService, TaskService>();
            this.services.AddTransient<IRepositoryService, RepositoryService>();
            this.services.AddTransient<IRuntimeService, ExecutionService>();
        }

        protected virtual void AddInternalServices()
        {
            //add core services.
            this.services.AddTransient<IDeploymentManager, DeploymentManager>();
            this.services.AddTransient<IHumanTaskHandler, HumanTaskHandler>();
            //this.services.AddTransient<IRuntimeService, ExecutionService>();
        }

        public IProcessEngineBuilder AddDeploymentStore<TDeploymentRepository>()
        {
            this.services.AddTransient(typeof(IDeploymentStore), typeof(TDeploymentRepository));

            return this;
        }

        public IProcessEngineBuilder AddUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
        {
            this.services.AddScoped(typeof(IUnitOfWork), typeof(TUnitOfWork));

            return this;
        }

        public IProcessEngineBuilder AddUnitOfWork(Func<IServiceProvider, IUnitOfWork> factory)
        {
            this.services.AddScoped<IUnitOfWork>((sp) => factory(sp));

            return this;
        }

        public virtual IProcessEngine Build()
        {
            foreach (var action in this.configureActions)
                action(this.services);

            this.AddInternalServices();
            this.AddCoreServices();

            var engineServices = this.useExternalServices ? this.serviceProvider : this.services.BuildServiceProvider();
            return new ProcessEngine(engineServices);
        }

        public virtual IProcessEngineBuilder ConfigureServices(Action<IServiceCollection> configureAction)
        {
            this.configureActions.Add(configureAction);

            return this;
        }

        public virtual IServiceCollection Services
        {
            get => this.services;
        }

        private IServiceProvider serviceProvider;

        public IProcessEngineBuilder UseApplicationServices(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            return this;
        }
    }
}
