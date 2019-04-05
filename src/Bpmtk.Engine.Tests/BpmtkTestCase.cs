using Bpmtk.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests
{
    public abstract class BpmtkTestCase : IDisposable
    {
        protected readonly IProcessEngine engine;
        protected readonly ITestOutputHelper output;
        protected readonly IContext context;
        protected readonly IRepositoryService repositoryService;
        protected readonly IRuntimeService runtimeService;
        protected readonly ITaskService taskService;
        protected readonly IUnitOfWork unitOfWork;

        public BpmtkTestCase(ITestOutputHelper output)
        {
            this.output = output;
            this.engine = this.BuildProcessEngine();

            this.context = this.engine.CreateContext();
            Context.SetCurrent(context);

            this.repositoryService = context.GetService<IRepositoryService>();
            this.runtimeService = context.GetService<IRuntimeService>();
            this.taskService = context.GetService<ITaskService>();

            this.unitOfWork = context.GetService<IUnitOfWork>();
        }

        protected virtual IProcessEngine BuildProcessEngine()
        {
            IProcessEngineBuilder builder = new ProcessEngineBuilder();
            builder.ConfigureServices(services =>
            {
                //services.Add<IDeploymentManager>()
            });

            return builder.AddDefaultStores(cfg =>
            {
                cfg.SetInterceptor(new XUnitSqlCaptureInterceptor(this.output));
            }).Build();
        }

        protected virtual void DeployBpmnModel(string resourceName)
        {
            var deploymentBuilder = this.repositoryService.CreateDeploymentBuilder();

            using (var ms = new MemoryStream())
            {
                var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
                stream.CopyTo(ms);
                stream.Close();

                var deployment = deploymentBuilder.SetBpmnModel(ms.ToArray())
                    .SetName("unit-tests")
                    .SetCategory("tests")
                    .Build();
            }
        }

        protected virtual void AssertProcessInstanceEnd(long id)
        {
            var pi = this.runtimeService.FindProcessInstanceById(id);
            Assert.True(pi.State == Runtime.ExecutionState.Completed);
        }

        [Fact]
        public abstract void Execute();

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
