using System;
using Bpmtk.Engine.Tasks;

namespace Bpmtk.Engine.WebApi.Models
{
    public class TaskInstanceModel
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual long? ProcessInstanceId
        {
            get;
            set;
        }

        //public virtual long? ActivityInstanceId
        //{
        //    get;
        //    set;
        //}

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string ActivityId
        {
            get;
            set;
        }

        public virtual short Priority
        {
            get;
            set;
        }

        public virtual string PriorityName
        {
            get;
            set;
        }

        public virtual string State
        {
            get;
            set;
        }

        public virtual string StateName
        {
            get;
            set;
        }

        public virtual string Assignee
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public static TaskInstanceModel Create(ITaskInstance task)
        {
            var model = new TaskInstanceModel();

            model.Id = task.Id;
            model.Name = task.Name;
            model.Created = task.Created;
            model.State = task.State.ToString();
            model.StateName = task.State.ToString();
            model.Priority = task.Priority;
            model.Assignee = task.Assignee;
            model.ProcessInstanceId = task.ProcessInstanceId;
            //model.ActivityInstanceId = task.ActivityInstance?.Id;
            
            return model;
        }
    }
}
