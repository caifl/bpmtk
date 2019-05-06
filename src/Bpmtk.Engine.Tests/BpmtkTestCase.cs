using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Storage;

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
        protected readonly ILoggerFactory loggerFactory = new LoggerFactory();
        protected ITransaction transaction;

        public BpmtkTestCase(ITestOutputHelper output)
        {
            var loggerProvider = new XunitLoggerProvider(output);
            this.loggerFactory.AddProvider(loggerProvider);

            this.output = output;
            this.engine = this.BuildProcessEngine();

            this.context = this.engine.CreateContext();
            Context.SetCurrent(context);

            this.deploymentManager = context.DeploymentManager;
            this.runtimeManager = context.RuntimeManager;
            this.taskManager = context.TaskManager;
            this.identityManager = context.IdentityManager;

            this.transaction = context.DbSession.BeginTransaction();

            var user = this.identityManager.FindUserById("felix");
            if (user == null)
            {
                user = new User() { Id = "felix", Name = "felix" };
                this.identityManager.CreateUser(user);
            }

            var group = this.identityManager.FindGroupById("tests");
            if (group == null)
            {
                group = new Group() { Id = "tests", Name = "tests" };
                group.Users = new List<UserGroup>();
                group.Users.Add(new UserGroup() { User = user });

                this.identityManager.CreateGroup(group);
            }

            this.context.SetAuthenticatedUser(user.Id);
        }

        protected virtual IProcessEngine BuildProcessEngine()
        {
            var contextFactory = new ContextFactory();
            contextFactory.Configure(builder =>
            {
               // builder.UseLoggerFactory(loggerFactory);
                builder.UseLazyLoadingProxies(true);
                builder.UseMySql("server=localhost;uid=root;pwd=123456;database=bpmtk3");
            });

            var engine = new ProcessEngineBuilder()
                .SetContextFactory(contextFactory)
                .SetLoggerFactory(loggerFactory)
                .Build();

            return engine;

            //return builder.AddHibernateStores(cfg =>
            //{
            //    cfg.SetInterceptor(new XUnitSqlCaptureInterceptor(this.output));
            //}).Build();
        }

        protected virtual Task DeployBpmnModel(string resourceName)
        {
            var deploymentBuilder = this.deploymentManager.CreateDeploymentBuilder();

            using (var ms = new MemoryStream())
            {
                var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
                Assert.NotNull(stream);

                stream.CopyTo(ms);
                stream.Close();

                var deployment = deploymentBuilder.SetBpmnModel(ms.ToArray())
                    .SetName("unit-tests")
                    .SetCategory("tests")
                    .Build();
            }

            return Task.CompletedTask;
        }

        protected virtual void AssertProcessEnded(long id)
        {
            var pi = this.runtimeManager.Find(id);
            Assert.True(pi.State == ExecutionState.Completed);
        }

        protected virtual void Commit() => this.transaction.Commit();

        public void Dispose()
        {
            this.transaction.Dispose();
            this.context.Dispose();
        }
    }
}
