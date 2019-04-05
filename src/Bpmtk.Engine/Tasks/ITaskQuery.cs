using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Tasks
{
    public interface ITaskQuery
    {
        ITaskQuery SetPriority(short priority);

        ITaskQuery SetMinPriority(short priority);

        ITaskQuery SetMaxPriority(short priority);

        ITaskQuery SetProcessInstanceId(long processInstanceId);

        ITaskQuery SetProcessDefinitionId(int processDefinitionId);

        ITaskQuery SetProcessDefinitionName(string processDefinitionName);

        ITaskQuery SetProcessDefinitionKey(string processDefinitionKey);

        ITaskQuery SetName(string name);

        ITaskQuery SetActivityId(string activityId);

        ITaskQuery SetAssignee(int assigneeId);

        //ITaskQuery SetOwner(int ownerId);

        //ITaskQuery SetDepartmentId(int? departmentId);

        //ITaskQuery SetOrganizationId(int? organizationId);

        ITaskQuery SetState(TaskState state);

        ITaskQuery SetCreatedFrom(DateTime created);

        ITaskQuery SetCreatedTo(DateTime created);

        //ITaskQuery WithProcessInstance();

        TaskInstance SingleResult();

        IList<TaskInstance> List();

        IList<TaskInstance> List(int count);

        IList<TaskInstance> List(int pageIndex, int pageSize);
    }
}
