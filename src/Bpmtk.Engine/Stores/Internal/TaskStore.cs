using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.Stores.Internal
{
    public class TaskStore : ITaskStore
    {
        protected readonly ISession session;

        public TaskStore(ISession session)
        {
            this.session = session;
        }

        public TaskInstance Find(long id)
            => this.session.Get<TaskInstance>(id);

        public virtual int GetActiveTaskCount(long tokenId)
        {
            return this.session.Query<TaskInstance>()
                .Where(x => x.TokenId == tokenId
                    && x.State == TaskState.Active)
                .Count();
        }
    }
}
