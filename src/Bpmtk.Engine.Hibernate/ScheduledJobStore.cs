using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Scheduler;
using NHibernate;
using NHibernate.Linq;

namespace Bpmtk.Engine
{
    public class ScheduledJobStore : IScheduledJobStore
    {
        public ScheduledJobStore(ISession session)
        {
            Session = session;
        }

        public virtual ISession Session { get; }

        public virtual async Task CreateAsync(params ScheduledJob[] scheduledJobs)
        {
            foreach (var entity in scheduledJobs)
                await this.Session.SaveAsync(entity);
        }

        public virtual Task<ScheduledJob> FindByKeyAsync(string key)
        {
            return this.Session.Query<ScheduledJob>().Where(x => x.Key == key)
                .SingleOrDefaultAsync();
        }

        public virtual async Task RemoveAsync(params ScheduledJob[] scheduledJobs)
        {
            foreach (var entity in scheduledJobs)
                await this.Session.DeleteAsync(entity);
        }

        public virtual async Task<IList<ScheduledJob>> GetByProcessDefinitionAsync(int processDefinitionId)
        {
            return await this.Session.Query<ScheduledJob>()
                .Where(x => x.ProcessDefinition.Id == processDefinitionId)
                .ToListAsync();
        }
    }
}
