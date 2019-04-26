using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Tasks;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Hibernate
{
    public class TaskStore : ITaskStore
    {
        protected readonly ISession session;

        public virtual IQueryable<TaskInstance> Tasks => this.session.Query<TaskInstance>();

        public TaskStore(ISession session)
        {
            this.session = session;
        }

        public virtual int GetActiveTaskCount(long tokenId)
        {
            return this.session.Query<TaskInstance>()
                .Where(x => x.Token.Id == tokenId
                    && x.State == TaskState.Active)
                .Count();
        }

        //public virtual ITaskQuery CreateQuery()
        //    => new TaskQuery(this.session);

        public virtual Task CreateAsync(TaskInstance task)
            => this.session.SaveAsync(task);

        public Task RemoveAsync(TaskInstance task)
            => this.session.DeleteAsync(task);

        public virtual async Task UpdateAsync(TaskInstance task)
        {
            await this.session.MergeAsync(task);
        }

        public virtual Task<TaskInstance> FindAsync(long taskId)
        {
            return this.session.GetAsync<TaskInstance>(taskId);
        }
    }
}
