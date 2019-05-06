using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Scheduler
{
    public class ScheduledJobManager : IScheduledJobManager
    {
        private readonly Context context;
        protected IDbSession session;

        public ScheduledJobManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;
        }

        public virtual Task<ScheduledJob> FindByKeyAsync(string key)
        {
            var query = this.session.ScheduledJobs.Where(x => x.Key == key);

            return this.session.QuerySingleAsync(query);
        }

        public virtual ScheduledJob FindByKey(string key)
        {
            var db = context.DbSession;
            var query = db.ScheduledJobs.Where(x => x.Key == key);

            return query.SingleOrDefault();
        }
    }
}
