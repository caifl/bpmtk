using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
       
        public static ProcessInstanceModel Create(ProcessInstance instance,
            int? initiatorId,
            string initiator)
        {
            var model = new ProcessInstanceModel();

            model.Id = instance.Id;
            model.Key = instance.Key;
            model.Name = instance.Name;
            model.State = instance.State.ToString();
            model.StateName = instance.State.ToString();
            model.InitiatorId = initiatorId;
            model.Initiator = initiator;
            model.StartTime = instance.StartTime;
            model.LastStateTime = instance.LastStateTime;
            model.Description = instance.Description;

            return model;
        }
    }
}
