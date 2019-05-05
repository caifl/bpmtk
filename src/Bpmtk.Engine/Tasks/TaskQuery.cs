using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Tasks
{
    public class TaskQuery : ITaskQuery
    {
        protected readonly IDbSession session;

        protected bool fetchAssignee;

        protected long? id;
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

            if (this.fetchAssignee)
                query = this.session.Fetch(query, x => x.Assignee);

            if (this.id != null)
                return query = query.Where(x => x.Id == this.id);

            if (this.assigneeId != null)
                query = query.Where(x => x.Assignee.Id == this.assigneeId);

            if (this.state != null)
                query = query.Where(x => x.State == this.state);

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

            if (this.name != null)
                query = query.Where(x => x.Name.Contains(this.name));
             
            return query;
        }

        public virtual ITaskQuery FetchAssignee()
        {
            this.fetchAssignee = true;

            return this;
        }

        public virtual ITaskQuery SetId(long id)
        {
            this.id = id;

            return this;
        }

        //public virtual IList<TaskInstance> List()
        //{
        //    return this.CreateNativeQuery().ToList();
        //}

        //public virtual IList<TaskInstance> List(int count)
        //{
        //    var query = this.CreateNativeQuery()
        //        .Take(count);

        //    return query.ToList();
        //}

        //public virtual IList<TaskInstance> List(int page, int pageSize)
        //{
        //    if (page < 1)
        //        page = 1;

        //    var count = pageSize;
        //    var skips = (page - 1) * pageSize;

        //    var query = this.CreateNativeQuery()
        //        .OrderByDescending(x => x.Created)
        //        .Skip(skips)
        //        .Take(count);

        //    return query.ToList();
        //}

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

        //public virtual TaskInstance Single()
        //{
        //    var query = this.CreateNativeQuery();

        //    return query.SingleOrDefault();
        //}

        public virtual Task<int> CountAsync()
        {
            var query = this.CreateNativeQuery();

            return this.session.CountAsync(query);
        }

        public virtual Task<TaskInstance> SingleAsync()
        {
            var query = this.CreateNativeQuery();

            return this.session.QuerySingleAsync(query);
        }

        public virtual Task<IList<TaskInstance>> ListAsync()
        {
            var query = this.CreateNativeQuery();

            return this.session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<TaskInstance>> ListAsync(int count)
        {
            var query = this.CreateNativeQuery().Take(count);

            return this.session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<TaskInstance>> ListAsync(int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            var count = pageSize;
            var skips = (page - 1) * pageSize;

            var query = this.CreateNativeQuery()
                .OrderByDescending(x => x.Created)
                .Skip(skips)
                .Take(count);

            return this.session.QueryMultipleAsync(query);
        }

        async Task<ITaskInstance> ITaskQuery.SingleAsync()
            => await this.SingleAsync();

        async Task<IList<ITaskInstance>> ITaskQuery.ListAsync()
        {
            var list = await this.ListAsync();
            return list.ToList<ITaskInstance>();
        }

        async Task<IList<ITaskInstance>> ITaskQuery.ListAsync(int count)
        {
            var list = await this.ListAsync(count);
            return list.ToList<ITaskInstance>();
        }

        async Task<IList<ITaskInstance>> ITaskQuery.ListAsync(int page, int pageSize)
        {
            var list = await this.ListAsync(page, pageSize);
            return list.ToList<ITaskInstance>();
        }
    }
}
