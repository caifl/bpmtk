using System;
using System.Collections.Generic;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine
{
    public interface ITaskService
    {
        ITaskInstance Find(long id);

        void AddUserPotentialOwner(long taskId, int userId, string type);

        ITaskQuery CreateQuery();

        void Complete(long id, IDictionary<string, object> variables = null);
    }
}
