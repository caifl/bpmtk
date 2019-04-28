using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public virtual string Name
        {
            get;
            set;
        }

        public static TaskModel Create(TaskInstance task)
        {
            var model = new TaskModel();
            model.Id = task.Id;
            model.Name = task.Name;
            
            return model;
        }
    }
}
