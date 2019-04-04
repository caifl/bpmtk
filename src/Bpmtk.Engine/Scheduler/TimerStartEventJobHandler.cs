using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Scheduler
{
    class TimerStartEventJobHandler : IScheduledJobHandler
    {
        public Task Execute(ScheduledJob job, IContext context)
        {
            return Task.CompletedTask;
        }
    }
}
