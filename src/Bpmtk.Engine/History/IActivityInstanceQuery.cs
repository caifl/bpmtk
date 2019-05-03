using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.History
{
    public interface IActivityInstanceQuery
    {
        IActivityInstanceQuery SetId(long id);

        IActivityInstanceQuery SetProcessInstanceId(long processInstanceId);

        IActivityInstanceQuery SetActivityType(string activityType);

        IActivityInstanceQuery SetName(string name);

        IActivityInstanceQuery SetState(ExecutionState state);

        IActivityInstanceQuery SetActivityId(string activityId);

        IActivityInstanceQuery SetIsMIRoot(bool isMIRoot);

        Task<ActivityInstance> SingleAsync();

        Task<IList<ActivityInstance>> ListAsync();
    }
}
