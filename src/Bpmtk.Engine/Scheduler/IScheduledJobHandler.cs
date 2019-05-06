using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Scheduler
{
    public interface IScheduledJobHandler
    {
        Task Execute(IContext context, ScheduledJob job);
    }
}
