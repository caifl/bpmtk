using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Models
{
    public class Deployment
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

        public virtual string Memo
        {
            get;
            set;
        }
    }
}
