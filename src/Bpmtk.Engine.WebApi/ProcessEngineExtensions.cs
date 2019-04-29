
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Bpmtk.Engine
{
    public static class ProcessEngineExtensions
    {
        public static IProcessEngineBuilder AddProcessEngine(this IServiceCollection services)
        {
            var builder = new ProcessEngineBuilder();

            services.AddSingleton(x =>
            {
                var loggerFactory = x.GetRequiredService<ILoggerFactory>();
                var httpContextAccessor = x.GetRequiredService<IHttpContextAccessor>();

                var contextFactory = new PerRequestContextFactory(httpContextAccessor);
                //sessionFactory.Configure(builder =>
                //{
                //    builder.UseLoggerFactory(loggerFactory);
                //    builder.UseLazyLoadingProxies(true);
                //    builder.UseMySql("server=localhost;uid=root;pwd=123456;database=bpmtk2");
                //});

                var engine = new ProcessEngineBuilder()
                    .SetContextFactory(contextFactory)
                    .SetLoggerFactory(loggerFactory)
                    .Build();

                return engine;
            });

            //Register context.
            services.AddScoped<IContext>(s =>
            {
                var engine = s.GetRequiredService<IProcessEngine>();
                return engine.CreateContext();
            });

            return builder;
        }
    }
}
