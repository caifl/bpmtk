using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IScheduledJobManager
    {
        ScheduledJob FindByKey(string key);

        Task<ScheduledJob> FindByKeyAsync(string key);
    }
}
