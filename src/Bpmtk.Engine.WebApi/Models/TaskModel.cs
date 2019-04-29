using System;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class TaskModel
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

        public virtual int State
        {
            get;
            set;
        }

        public virtual string StateName
        {
            get;
            set;
        }

        public virtual int? AssigneeId
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

        public static TaskModel Create(TaskInstance task)
        {
            var model = new TaskModel();

            model.Id = task.Id;
            model.Name = task.Name;
            model.Created = task.Created;
            model.State = (int)task.State;
            model.StateName = task.State.ToString();
            model.Priority = task.Priority;
            model.ProcessInstanceId = task.ProcessInstanceId;
            //model.ActivityInstanceId = task.ActivityInstance?.Id;
            
            return model;
        }
    }
}
