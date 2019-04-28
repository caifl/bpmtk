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

        public virtual DateTime? ValidFrom
        {
            get;
            set;
        }

        public virtual DateTime? ValidTo
        {
            get;
            set;
        }

        public virtual int Version
        {
            get;
            set;
        }

        public virtual string VersionTag
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
            model.State = definition.State.ToString();
            model.StateName = definition.State.ToString();
            model.ValidFrom = definition.ValidFrom;
            model.ValidTo = definition.ValidTo;
            model.Version = definition.Version;
            model.VersionTag = definition.VersionTag;

            return model;
        }
    }
}
