using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.WebApi.Models
{
    public class DeploymentModel
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

        public static DeploymentModel Create(Deployment deployment)
        {
            var model = new DeploymentModel();

            model.Id = deployment.Id;
            model.Name = deployment.Name;

            return model;
        }
    }
}
