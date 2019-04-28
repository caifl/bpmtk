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

        public static ProcessInstanceModel Create(ProcessInstance instance)
        {
            var model = new ProcessInstanceModel();

            model.Id = instance.Id;
            model.Key = instance.Key;
            model.Name = instance.Name;

            return model;
        }
    }
}
