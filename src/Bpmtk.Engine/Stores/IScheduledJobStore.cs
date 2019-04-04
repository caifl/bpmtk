using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Scheduler;

namespace Bpmtk.Engine.Stores
{
    public interface IScheduledJobStore
    {
        void AddRange(IEnumerable<ScheduledJob> items);

        Task<ScheduledJob> FindByKeyAsync(string key);

        IEnumerable<ScheduledJob> GetScheduledJobsByProcess(int processDefinitionId);

        void RemoveRange(IEnumerable<ScheduledJob> items);
    }
}
