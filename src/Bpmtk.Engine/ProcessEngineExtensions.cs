using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Bpmtk.Engine
{
    public static class ProcessEngineExtensions
    {
        public static IProcessEngineBuilder AddProcessEngine(this IServiceCollection services)
        {
            var builder = new ProcessEngineBuilder(services);

            services.AddSingleton<IProcessEngine>(x =>
            {
                builder.UseApplicationServices(x);
                return builder.Build();
            });

            return builder;
        }
    }
}
