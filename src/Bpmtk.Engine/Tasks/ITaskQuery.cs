using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskQuery
    {
        ITaskQuery FetchAssignee();

        ITaskQuery SetId(long id);

        ITaskQuery SetPriority(short priority);

        ITaskQuery SetMinPriority(short priority);

        ITaskQuery SetMaxPriority(short priority);

        ITaskQuery SetProcessInstanceId(long processInstanceId);

        ITaskQuery SetProcessDefinitionId(int processDefinitionId);

        ITaskQuery SetProcessDefinitionName(string processDefinitionName);

        ITaskQuery SetProcessDefinitionKey(string processDefinitionKey);

        ITaskQuery SetName(string name);

        ITaskQuery SetActivityId(string activityId);

        ITaskQuery SetAssignee(string assignee);

        //ITaskQuery SetOwner(int ownerId);

        //ITaskQuery SetDepartmentId(int? departmentId);

        //ITaskQuery SetOrganizationId(int? organizationId);

        ITaskQuery SetState(TaskState state);

        ITaskQuery SetCreatedFrom(DateTime created);

        ITaskQuery SetCreatedTo(DateTime created);

        //TaskInstance Single();

        //IList<TaskInstance> List();

        //IList<TaskInstance> List(int count);

        //IList<TaskInstance> List(int pageIndex, int pageSize);

        Task<ITaskInstance> SingleAsync();

        Task<int> CountAsync();

        IList<ITaskInstance> List();

        Task<IList<ITaskInstance>> ListAsync();

        Task<IList<ITaskInstance>> ListAsync(int count);

        Task<IList<ITaskInstance>> ListAsync(int page, int pageSize);
    }
}
