using System;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Repository
{
    public class ProcessDefinition
    {
        protected ICollection<DefinitionIdentityLink> identityLinks;

        protected ProcessDefinition()
        {}

        public ProcessDefinition(Deployment deployment, 
            Process process, 
            bool hasDiagram,
            int version)
        {
            this.identityLinks = new List<DefinitionIdentityLink>();
            this.Deployment = deployment;
            this.DeploymentId = deployment.Id;
            this.TenantId = deployment.TenantId;
            this.Version = version;

            this.Key = process.Id;
            this.Name = StringHelper.Get(process.Name, 100, process.Id);
            this.HasDiagram = hasDiagram;
            this.State = ProcessDefinitionState.Active;
            this.Created = deployment.Created;
            this.Modified = this.Created;
            this.Category = deployment.Category;

            if (process.Documentations.Count > 0)
            {
                var textArray = process.Documentations.Select(x => x.Text).ToArray();
                this.Description = StringHelper.Join(textArray, "\n", 100);
            }
        }

        public virtual void AddIdentityLink(params DefinitionIdentityLink[] identityLinks)
        {
            if (identityLinks == null)
                throw new ArgumentNullException(nameof(identityLinks));

            foreach (var identityLink in identityLinks)
            {
                identityLink.ProcessDefinition = this;
                this.identityLinks.Add(identityLink);
            }
        }

        public virtual IReadOnlyList<IdentityLink> IdentityLinks => this.identityLinks.ToList(); 

        public virtual int Id
        {
            get;
            protected set;
        }

        public virtual string TenantId
        {
            get;
            set;
        }

        public virtual string Category
        {
            get;
            set;
        }

        public virtual int DeploymentId
        {
            get;
            protected set;
        }

        public virtual Deployment Deployment
        {
            get;
            protected set;
        }

        //public virtual Package Package
        //{
        //    get;
        //    protected set;
        //}

        //public virtual Package Package
        //{
        //    get => this.package;
        //}

        //public virtual void AddResourceLinks(params DefinitionResourceLink[] resourceLinks)
        //{
        //    this.resourceLinks.AddRange(resourceLinks);
        //}

        //public virtual bool RemoveResourceLink(DefinitionResourceLink resourceLink)
        //{
        //    return this.resourceLinks.Remove(resourceLink);
        //}

        //public virtual IReadOnlyCollection<DefinitionResourceLink> ResourceLinks
        //{
        //    get => this.resourceLinks.AsReadOnly();
        //}

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual string Key
        {
            get;
            protected set;
        }

        public virtual int Version
        {
            get;
            protected set;
        }

        public virtual DateTime Created
        {
            get;
            protected set;
        }

        public virtual DateTime Modified
        {
            get;
            set;
        }

        public virtual bool HasDiagram
        {
            get;
            protected set;
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

        public virtual void Activate()
        {
            this.State = ProcessDefinitionState.Active;
        }

        public virtual void Inactivate()
        {
            this.State = ProcessDefinitionState.Inactive;
        }

        public virtual void VerifyState()
        {
            var date = Clock.Now;

            if (ValidTo.HasValue && ValidTo > date)
            {
                Inactivate();
                return;
            }

            if (ValidFrom.HasValue && ValidFrom < date)
            {
                Inactivate();
                return;
            }

            this.Activate();
        }

        public virtual string ConcurrencyStamp
        {
            get;
            set;
        }

        public virtual ProcessDefinitionState State
        {
            get;
            set;
        }

        public virtual string VersionTag
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }
    }

    public enum ProcessDefinitionState : int
    {
        Inactive = 0,

        Active = 1
    }
}
