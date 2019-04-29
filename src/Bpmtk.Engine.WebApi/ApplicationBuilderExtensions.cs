using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bpmtk.Engine.WebApi
{
    static class ApplicationBuilderExtensions
    {
        public static void UseProcessEngine(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var services = context.RequestServices;

                var bpmContext = services.GetRequiredService<IContext>();

                //Set current context.
                Context.SetCurrent(bpmContext);

                bpmContext.SetAuthenticatedUser(100);
                //session.SetAuthenticatedUser(4);
                // Do work that doesn't write to the Response.
                //if (context.User.Identity.IsAuthenticated)
                //{
                //    var nameId = System.Security.Claims.ClaimTypes.NameIdentifier;
                //    var value = context.User.Claims.Where(x => x.Type == nameId)
                //        .Select(x => x.Value)
                //        .SingleOrDefault();
                //    int userId = 0;

                //    if (int.TryParse(value, out userId))
                //        session.SetAuthenticatedUser(userId);
                //}

                await next.Invoke();
            });
        }
    }
}
