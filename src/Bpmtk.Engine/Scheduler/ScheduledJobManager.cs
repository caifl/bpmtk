using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Scheduler
{
    class ScheduledJobManager : IScheduledJobManager
    {
        private readonly Context context;

        public ScheduledJobManager(Context context)
        {
            this.context = context;
        }

        public virtual Task<ScheduledJob> FindByKeyAsync(string key)
        {
            var db = context.DbSession;
            var query = db.ScheduledJobs.Where(x => x.Key == key);

            return db.QuerySingleAsync(query);
        }
    }
}
