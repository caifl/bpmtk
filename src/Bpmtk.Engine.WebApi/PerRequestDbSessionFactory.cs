
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Bpmtk.Engine.Models;
using Microsoft.AspNetCore.Http;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine
{
    public class PerRequestDbSessionFactory : IDbSessionFactory
    {
        public PerRequestDbSessionFactory(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public virtual IDbSession Create()
        {
            var context = this.HttpContextAccessor.HttpContext;
            var db = context.RequestServices.GetService<BpmDbContext>();
            return new DbSession(db);
        }
    }
}
