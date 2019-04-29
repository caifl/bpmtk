
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine
{
    public class PerRequestContextFactory : IContextFactory
    {
        public PerRequestContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public virtual IContext Create(IProcessEngine engine)
        {
            var context = this.HttpContextAccessor.HttpContext;
            var db = context.RequestServices.GetService<BpmDbContext>();
            return new Context(engine, new DbSession(db));
        }
    }
}
