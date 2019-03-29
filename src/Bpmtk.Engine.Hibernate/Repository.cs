using System;
using System.Collections.Generic;
using NHibernate;

namespace Bpmtk.Engine
{
    public abstract class Repository<TEntity, TKey>
    {
        protected readonly ISession session;

        public Repository(ISession session)
        {
            this.session = session;
        }

        public virtual TEntity Find(TKey id)
        {
            return this.session.Get<TEntity>(id);
        }

        public virtual void Add(TEntity entity)
        {
            this.session.Save(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.session.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            this.session.Delete(entity);
        }
    }
}
