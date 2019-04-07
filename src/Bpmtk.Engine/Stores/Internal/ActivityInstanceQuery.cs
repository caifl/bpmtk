using Bpmtk.Engine.Runtime;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Engine.Stores.Internal
{
    class ActivityInstanceQuery : IActivityInstanceQuery
    {
        private readonly ISession session;

        public ActivityInstanceQuery(ISession session)
        {
            this.session = session;
        }

        public IList<ActivityInstance> List()
        {
            return this.session.Query<ActivityInstance>().ToList();
        }
    }
}
