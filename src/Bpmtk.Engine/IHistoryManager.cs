using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine
{
    public interface IHistoryManager
    {
        IQueryable<ActivityInstance> ActivityInstances
        {
            get;
        }

        IActivityInstanceQuery CreateActivityQuery();

        Task<IList<ActivityInstance>> GetActivityInstancesAsync(long processInstanceId);

        Task RecordActivityReadyAsync(ExecutionContext executionContext,
            IList<Token> joinedTokens);

        Task RecordActivityStartAsync(ExecutionContext executionContext);

        Task RecordActivityEndAsync(ExecutionContext executionContext);
    }
}
