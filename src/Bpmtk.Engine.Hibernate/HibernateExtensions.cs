using NHibernate;
using NHibernate.Cfg;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Tool.hbm2ddl;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Scheduler;
using Bpmtk.Engine.Tasks;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine
{
    public static class HibernateExtensions
    {
        public static void AddHibernate(this IServiceCollection services,
            Action<HibernateOptions> optionsAction)
        {
            var options = new HibernateOptions();
            optionsAction(options);
        }

        public static void AddHibernate(this IServiceCollection services,
            Action<Configuration> configureAction = null)
        {
            services.AddScoped(x => x.GetRequiredService<ISessionFactory>().OpenSession());
            services.AddSingleton(x =>
            {
                var mappingsDir = new DirectoryInfo(@".\Mappings");
                var cfg = new Configuration()
                        .Configure(@".\nhibernate.cfg.xml")
                        .AddDirectory(mappingsDir);

                if (configureAction != null)
                    configureAction(cfg);

                DumpSchema(cfg);

                var factory = cfg.BuildSessionFactory();
                return factory;
            });
        }

        static void DumpSchema(Configuration cfg)
        {
            //StreamWriter writer = new StreamWriter(@"C:\Users\Felix\bpmtk_create.sql");
            var export = new SchemaExport(cfg);
            export.Create(x =>
            {
                //var sql = x;
                ///writer.WriteLine(sql);
                //writer.Flush();
            }, true);
            //export.Execute(true, true, false);

            //writer.Flush();
            //writer.Close();
        }

        public static IProcessEngineBuilder AddHibernateStores(this IProcessEngineBuilder builder,
            Action<Configuration> configureAction = null)
        {
            var services = builder.Services;

            //builder.ConfigureServices(services =>
            //{
                AddHibernate(services, configureAction);
            //});

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Add Stores.
            services.AddTransient<IDeploymentStore, DeploymentStore>();
            services.AddTransient<IEventSubscriptionStore, EventSubscriptionStore>();
            services.AddTransient<IScheduledJobStore, ScheduledJobStore>();
            services.AddTransient<ITaskStore, TaskStore>();
            services.AddTransient<IInstanceStore, InstanceStore>();
            services.AddTransient<IIdentityStore, IdentityStore>();

            return builder;
        }
    }
}
