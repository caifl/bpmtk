using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Scheduler
{
    public interface IScheduledJobStore
    {
        Task CreateAsync(params ScheduledJob[] scheduledJobs);

        Task<ScheduledJob> FindByKeyAsync(string key);

        Task<IList<ScheduledJob>> GetByProcessDefinitionAsync(int processDefinitionId);

        Task RemoveAsync(params ScheduledJob[] scheduledJobs);
    }
}
