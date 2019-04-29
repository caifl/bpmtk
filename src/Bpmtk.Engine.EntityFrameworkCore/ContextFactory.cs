using System;
using Microsoft.EntityFrameworkCore;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine
{
    public class ContextFactory : IContextFactory
    {
        protected Action<DbContextOptionsBuilder> optionsBuilderAction;

        public virtual void Configure(Action<DbContextOptionsBuilder> optionsBuilderAction)
        {
            this.optionsBuilderAction = optionsBuilderAction;
        }

        public virtual IContext Create(IProcessEngine engine)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            optionsBuilderAction.Invoke(builder);

            var session = new DbSession(new BpmDbContext(builder.Options));

            return new Context(engine, session);
        }
    }
}
