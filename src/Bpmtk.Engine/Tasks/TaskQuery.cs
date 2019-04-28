using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Tasks
{
    public class TaskQuery : ITaskQuery
    {
        protected readonly IDbSession session;

        protected TaskState? state;
        protected string name;
        protected string activityId;
        protected short? priority;
        protected short? minPriority;
        protected short? maxPriority;
        protected int? assigneeId;
        protected long? processInstanceId;
        protected int? processDefinitionId;
        protected string processDefinitionKey;
        protected string processDefinitionName;

        protected DateTime? createdFrom;
        protected DateTime? createdTo;

        public TaskQuery(IDbSession session)
        {
            this.session = session;
        }

        protected virtual IQueryable<TaskInstance> CreateNativeQuery()
        {
            var query = this.session.Tasks;

            if (this.assigneeId != null)
                query = query.Where(x => x.Assignee.Id == this.assigneeId);

            if (this.processInstanceId != null)
                query = query.Where(x => x.ProcessInstance.Id == this.processInstanceId);

            //process-definition
            if (this.processDefinitionId != null)
                query = query.Where(x => x.ProcessInstance.ProcessDefinition.Id == this.processDefinitionId);
            else if(this.processDefinitionKey != null)
                query = query.Where(x => x.ProcessInstance.ProcessDefinition.Key == this.processDefinitionKey);
            else if (this.processDefinitionName != null)
                query = query.Where(x => x.ProcessInstance.ProcessDefinition.Name == this.processDefinitionName);

            //priority
            if (this.priority != null)
                query = query.Where(x => x.Priority == this.priority);
            else
            {
                if (this.minPriority != null)
                    query = query.Where(x => x.Priority >= this.minPriority);

                if (this.maxPriority != null)
                    query = query.Where(x => x.Priority <= this.maxPriority);
            }

            if (this.createdFrom != null)
                query = query.Where(x => x.Created >= this.createdFrom);

            if (this.createdTo != null)
                query = query.Where(x => x.Created <= this.createdTo);
             
            return query;
        }

        public virtual IList<TaskInstance> List()
        {
            return this.CreateNativeQuery().ToList();
        }

        public virtual IList<TaskInstance> List(int count)
        {
            var query = this.CreateNativeQuery()
                .Take(count);

            return query.ToList();
        }

        public virtual IList<TaskInstance> List(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public virtual ITaskQuery SetActivityId(string activityId)
        {
            this.activityId = activityId;

            return this;
        }

        public virtual ITaskQuery SetAssignee(int assigneeId)
        {
            this.assigneeId = assigneeId;

            return this;
        }

        public virtual ITaskQuery SetCreatedFrom(DateTime fromDate)
        {
            this.createdFrom = fromDate;

            return this;
        }

        public virtual ITaskQuery SetCreatedTo(DateTime toDate)
        {
            this.createdTo = toDate;

            return this;
        }

        public virtual ITaskQuery SetMaxPriority(short priority)
        {
            this.maxPriority = priority;

            return this;
        }

        public virtual ITaskQuery SetMinPriority(short priority)
        {
            this.minPriority = priority;

            return this;
        }

        public virtual ITaskQuery SetName(string name)
        {
            this.name = name;

            return this;
        }

        public virtual ITaskQuery SetPriority(short priority)
        {
            this.priority = priority;

            return this;
        }

        public virtual ITaskQuery SetProcessDefinitionId(int processDefinitionId)
        {
            this.processDefinitionId = processDefinitionId;

            return this;
        }

        public virtual ITaskQuery SetProcessDefinitionKey(string processDefinitionKey)
        {
            this.processDefinitionKey = processDefinitionKey;

            return this;
        }

        public virtual ITaskQuery SetProcessDefinitionName(string processDefinitionName)
        {
            this.processDefinitionName = processDefinitionName;

            return this;
        }

        public virtual ITaskQuery SetProcessInstanceId(long processInstanceId)
        {
            this.processInstanceId = processInstanceId;

            return this;
        }

        public virtual ITaskQuery SetState(TaskState state)
        {
            this.state = state;

            return this;
        }

        public virtual TaskInstance SingleResult()
        {
            var query = this.CreateNativeQuery();

            return query.SingleOrDefault();
        }
    }
}
