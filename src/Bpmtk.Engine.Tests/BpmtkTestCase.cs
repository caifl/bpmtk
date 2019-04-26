using Bpmtk.Infrastructure;
using System;
using System.Threading.Tasks;
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
        protected readonly IDeploymentManager deploymentManager;
        protected readonly IRuntimeManager runtimeManager;
        protected readonly ITaskManager taskManager;
        protected readonly IIdentityManager identityManager;
        protected readonly IUnitOfWork unitOfWork;

        public BpmtkTestCase(ITestOutputHelper output)
        {
            this.output = output;
            this.engine = this.BuildProcessEngine();

            this.context = this.engine.CreateContext();
            Context.SetCurrent(context);

            this.deploymentManager = context.DeploymentManager;
            this.runtimeManager = context.RuntimeManager;
            this.taskManager = context.TaskManager;
            this.identityManager = context.IdentityManager;

            this.unitOfWork = context.GetService<IUnitOfWork>();

            var user = new Models.User() { Name = "felix" };
            this.identityManager.CreateUserAsync(user).GetAwaiter().GetResult();

            this.context.SetAuthenticatedUser(user.Id);
        }

        protected virtual IProcessEngine BuildProcessEngine()
        {
            IProcessEngineBuilder builder = new ProcessEngineBuilder();
            builder.ConfigureServices(services =>
            {
                //services.Add<IDeploymentManager>()
            });

            return builder.AddHibernateStores(cfg =>
            {
                cfg.SetInterceptor(new XUnitSqlCaptureInterceptor(this.output));
            }).Build();
        }

        protected virtual async Task DeployBpmnModel(string resourceName)
        {
            var deploymentBuilder = this.deploymentManager.CreateDeploymentBuilder();

            using (var ms = new MemoryStream())
            {
                var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
                stream.CopyTo(ms);
                stream.Close();

                var deployment = await deploymentBuilder.SetBpmnModel(ms.ToArray())
                    .SetName("unit-tests")
                    .SetCategory("tests")
                    .BuildAsync();
            }
        }

        protected virtual void AssertProcessEnded(long id)
        {
            //var pi = this.runtimeService.fi(id);
            //Assert.True(pi.State == Runtime.ExecutionState.Completed);
        }

        [Fact]
        public abstract Task Execute();

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
