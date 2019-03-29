using System;
using NHibernate;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine
{
    public class ProcessInstanceRepository 
        : Repository<ProcessInstance, long>
    {
        public ProcessInstanceRepository(ISession session)
            : base(session)
        {
        }
    }
}
