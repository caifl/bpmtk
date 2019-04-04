using System;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Models;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Repository
{
    public class Deployment : IDeployment, IAggregateRoot
    {
        public Deployment(string name, byte[] modelData)
        {
            this.Name = name;
            this.Model = new ByteArray(modelData);
            this.Created = Clock.Now;
            this.ProcessDefinitions = new List<ProcessDefinition>();
        }

        protected Deployment()
        {
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

        public virtual void Add(ProcessDefinition processDefinition)
        {
            this.ProcessDefinitions.Add(processDefinition);
        }

        public virtual ICollection<ProcessDefinition> ProcessDefinitions
        {
            get;
            set;
        }

        public virtual string Memo
        {
            get;
            set;
        }
    }
}
