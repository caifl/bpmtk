using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Tasks;
using NHibernate;
using NHibernate.Linq;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Stores.Internal
{
    class TaskQuery : ITaskQuery
    {
        private readonly ISession session;

        protected long? processInstanceId;
        protected TaskState? state;
        protected string name;

        public TaskQuery(ISession session)
        {
            this.session = session;
        }

        public virtual IList<TaskInstance> List()
        {
            var query = this.session.Query<TaskInstance>();

            if (this.processInstanceId.HasValue)
                query = query.Where(x => x.ProcessInstanceId == this.processInstanceId.Value);

            if (this.state != null)
                query = query.Where(x => x.State == this.state.Value);

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<TaskInstance> List(int count)
        {
            throw new NotImplementedException();
        }

        public IList<TaskInstance> List(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetActivityId(string activityId)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetAssignee(int assigneeId)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetCreatedFrom(DateTime created)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetCreatedTo(DateTime created)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetMaxPriority(short priority)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetMinPriority(short priority)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetName(string name)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetPriority(short priority)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetProcessDefinitionId(int processDefinitionId)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetProcessDefinitionKey(string processDefinitionKey)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetProcessDefinitionName(string processDefinitionName)
        {
            throw new NotImplementedException();
        }

        public ITaskQuery SetProcessInstanceId(long processInstanceId)
        {
            this.processInstanceId = processInstanceId;

            return this;
        }

        public ITaskQuery SetState(TaskState state)
        {
            this.state = state;

            return this;
        }

        public TaskInstance SingleResult()
        {
            throw new NotImplementedException();
        }
    }
}
