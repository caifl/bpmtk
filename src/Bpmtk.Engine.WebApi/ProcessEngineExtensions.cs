
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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

                var sessionFactory = new PerRequestDbSessionFactory(httpContextAccessor);
                //sessionFactory.Configure(builder =>
                //{
                //    builder.UseLoggerFactory(loggerFactory);
                //    builder.UseLazyLoadingProxies(true);
                //    builder.UseMySql("server=localhost;uid=root;pwd=123456;database=bpmtk2");
                //});

                var engine = new ProcessEngineBuilder()
                    .SetDbSessionFactory(sessionFactory)
                    .SetLoggerFactory(loggerFactory)
                    .Build();

                return engine;
            });

            return builder;
        }
    }
}
