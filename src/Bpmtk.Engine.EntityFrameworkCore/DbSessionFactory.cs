using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine
{
    public class DbSessionFactory : Models.IDbSessionFactory
    {
        protected Action<DbContextOptionsBuilder> optionsBuilderAction;

        public virtual void Configure(Action<DbContextOptionsBuilder> optionsBuilderAction)
        {
            this.optionsBuilderAction = optionsBuilderAction;
        }

        public virtual IDbSession Create()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            optionsBuilderAction.Invoke(builder);

            return new DbSession(new BpmDbContext(builder.Options));
        }
    }
}
