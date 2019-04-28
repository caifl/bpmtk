using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ActivityInstanceModel
    {
        public virtual long Id
        {
            get;
            set;
        }

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

        public virtual string ActivityType
        {
            get;
            set;
        }

        public virtual DateTime? StartTime
        {
            get;
            set;
        }

        public static ActivityInstanceModel Create(ActivityInstance act)
        {
            var model = new ActivityInstanceModel();

            model.Id = act.Id;
            model.Name = act.Name;
            model.ActivityId = act.ActivityId;
            model.ActivityType = act.ActivityType;
            model.StartTime = act.StartTime;
            
            return model;
        }
    }
}
