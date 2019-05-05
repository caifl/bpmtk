
using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Identity;

namespace Bpmtk.Engine.Models
{
    public class Deployment : IDeployment
    {
        public Deployment()
        {
            this.ProcessDefinitions = new List<ProcessDefinition>();
        }

        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets category.
        /// </summary>
        public virtual string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets BPMN Meta-Model Data.
        /// </summary>
        public virtual ByteArray Model
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the deployed time.
        /// </summary>
        public virtual DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        public virtual int? PackageId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source package.
        /// </summary>
        public virtual Package Package
        {
            get;
            set;
        }

        public virtual ICollection<ProcessDefinition> ProcessDefinitions
        {
            get;
        }

        IReadOnlyList<IProcessDefinition> IDeployment.ProcessDefinitions
        {
            get => this.ProcessDefinitions.ToList();
        }

        IUser IDeployment.User => this.User;

        public virtual string Memo
        {
            get;
            set;
        }
    }
}
