using NHibernate;
using NHibernate.Cfg;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Bpmtk.Engine.Stores;
using Bpmtk.Engine.Stores.Internal;
using NHibernate.Tool.hbm2ddl;
using Bpmtk.Infrastructure;

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
                var mappingsDir = new DirectoryInfo(@".\Stores\Mappings");
                var cfg = new Configuration()
                        .Configure(@".\Stores\nhibernate.cfg.xml")
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
            StreamWriter writer = new StreamWriter(@"C:\Users\Felix\bpmtk_create.sql");
            var export = new SchemaExport(cfg);
            export.Create(x =>
            {
                var sql = x;
                writer.WriteLine(sql);
                writer.Flush();
            }, true);
            //export.Execute(true, true, false);

            writer.Flush();
            writer.Close();
        }

        public static IProcessEngineBuilder AddDefaultStores(this IProcessEngineBuilder builder,
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

            return builder;
        }
    }
}
