using System;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.History;
using Bpmtk.Engine.Runtime;
using System.Threading.Tasks;

namespace Bpmtk.Engine
{
    public interface IHistoryManager
    {
        IActivityInstanceQuery CreateActivityQuery();

        IList<ActivityInstance> GetActivityInstances(long processInstanceId);

        Task<IList<ActivityInstance>> GetActivityInstancesAsync(long processInstanceId);

        void RecordActivityReady(ExecutionContext executionContext);

        void RecordActivityStart(ExecutionContext executionContext);

        void RecordActivityEnd(ExecutionContext executionContext);
    }
}
