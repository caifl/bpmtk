using System;
using System.Collections.Generic;
using Bpmtk.Engine.Repository;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Models
{
    public class ProcessDefinition : IProcessDefinition
    {
        public ProcessDefinition()
        {
            this.IdentityLinks = new List<IdentityLink>();
        }

        //public virtual void AddIdentityLink(params IdentityLink[] identityLinks)
        //{
        //    if (identityLinks == null)
        //        throw new ArgumentNullException(nameof(identityLinks));

        //    foreach (var identityLink in identityLinks)
        //    {
        //        identityLink.ProcessDefinition = this;
        //        this.identityLinks.Add(identityLink);
        //    }
        //}

        public virtual ICollection<IdentityLink> IdentityLinks
        {
            get;
        }

        public virtual int Id
        {
            get;
            set;
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
            set;
        }

        public virtual Deployment Deployment
        {
            get;
            set;
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
            set;
        }

        public virtual string Key
        {
            get;
            set;
        }

        public virtual int Version
        {
            get;
            set;
        }

        public virtual DateTime Created
        {
            get;
            set;
        }

        public virtual DateTime Modified
        {
            get;
            set;
        }

        public virtual bool HasDiagram
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
}
