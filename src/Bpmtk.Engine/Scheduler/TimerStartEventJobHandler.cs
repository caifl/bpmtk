using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Scheduler
{
    class TimerStartEventJobHandler : IScheduledJobHandler
    {
        public virtual Task Execute(IContext context, ScheduledJob job)
        {
            return Task.CompletedTask;
        }
    }
}
