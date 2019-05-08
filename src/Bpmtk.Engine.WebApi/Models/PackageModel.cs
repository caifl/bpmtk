using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine.WebApi.Models
{
    public class PackageModel
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

        public static PackageModel Create(IPackage package)
        {
            var model = new PackageModel();

            model.Id = package.Id;
            model.Name = package.Name;
            model.Created = package.Created;

            return model;
        }
    }
}
