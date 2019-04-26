using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IScheduledJobManager
    {
        Task<ScheduledJob> FindByKeyAsync(string key);
    }
}
