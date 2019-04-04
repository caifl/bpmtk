using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Scheduler;
using NHibernate;

namespace Bpmtk.Engine.Stores.Internal
{
    class ScheduledJobStore : IScheduledJobStore
    {
        public ScheduledJobStore(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }

        public void AddRange(IEnumerable<ScheduledJob> items)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduledJob> FindByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScheduledJob> GetScheduledJobsByProcess(int processDefinitionId)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<ScheduledJob> items)
        {
            throw new NotImplementedException();
        }
    }
}
