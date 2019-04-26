using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Scheduler
{
    public interface IScheduledJobHandler
    {
        Task ExecuteAsync(IContext context, ScheduledJob job);
    }
}
