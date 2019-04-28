using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class ProcessDefinitionModel
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

        public static ProcessDefinitionModel Create(ProcessDefinition definition)
        {
            var model = new ProcessDefinitionModel();

            model.Id = definition.Id;
            model.Key = definition.Key;
            model.Name = definition.Name;

            return model;
        }
    }
}
