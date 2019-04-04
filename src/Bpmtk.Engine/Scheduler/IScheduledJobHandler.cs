using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Scheduler
{
    public interface IScheduledJobHandler
    {
        Task Execute(ScheduledJob job, IContext context);
    }
}
