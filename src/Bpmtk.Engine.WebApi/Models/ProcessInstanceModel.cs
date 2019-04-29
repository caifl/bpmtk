using System;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ProcessInstanceModel
    {
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
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

        public virtual int? InitiatorId
        {
            get;
            set;
        }

        public virtual string Initiator
        {
            get;
            set;
        }

        public virtual DateTime? StartTime
        {
            get;
            set;
        }

        public virtual DateTime? LastStateTime
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
       
        public static ProcessInstanceModel Create(ProcessInstance instance)
        {
            var model = new ProcessInstanceModel();

            model.Id = instance.Id;
            model.Key = instance.Key;
            model.Name = instance.Name;
            model.State = (int)instance.State;
            model.StateName = instance.State.ToString();
            model.InitiatorId = instance.Initiator?.Id;
            model.Initiator = instance.Initiator?.UserName;
            model.StartTime = instance.StartTime;
            model.LastStateTime = instance.LastStateTime;
            model.Description = instance.Description;

            return model;
        }
    }
}
