using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.WebApi.Models
{
    public class DeploymentModel
    {
        public virtual int Id
        {
            get;
            set;
        }

        //public virtual string Key
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public static DeploymentModel Create(IDeployment deployment)
        {
            var model = new DeploymentModel();

            model.Id = deployment.Id;
            model.Name = deployment.Name;
            model.Created = deployment.Created;

            return model;
        }
    }
}
