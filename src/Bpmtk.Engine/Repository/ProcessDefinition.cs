using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
//using Bpmtk.Bpmn2;
using Bpmtk.Infrastructure;
using Bpmtk.Engine.Models;
//using Bpmtk.Bpmn2.Parser;

namespace Bpmtk.Engine.Repository
{
    public class ProcessDefinition : IAggregateRoot
    {
        protected ICollection<DefinitionIdentityLink> identityLinks;

        protected ByteArray model;
        //protected readonly static ConcurrentDictionary<int, Process> modelCache = new ConcurrentDictionary<int, Process>();

        //protected Package package;
        //protected Model model;
        //protected Process process;
        //protected List<DefinitionResourceLink> resourceLinks = new List<DefinitionResourceLink>();

        protected ProcessDefinition()
        {

        }

        public virtual IEnumerable<IdentityLink> IdentityLinks => this.identityLinks.AsEnumerable<IdentityLink>(); 

        //public ProcessDefinition(
        //    Package package, 
        //    Model model,
        //    Process process,
        //    int ownerId,
        //    int version)
        //{
        //    this.package = package;
        //    this.PackageId = package.Id;
        //    this.model = model;
        //    this.process = process;

        //    this.Key = process.Id;
        //    this.OwnerId = ownerId;
        //    this.Name = process.Name;

        //    if (string.IsNullOrEmpty(this.Name))
        //        this.Name = this.Key;

        //    this.Version = version;
        //    this.Created = DateTime.Now;
        //    this.Modified = this.Created;
        //    this.OrganizationId = package.OrganizationId;
        //    this.CategoryId = package.CategoryId;
        //    this.HasDiagram = process.HasDiagram;
        //    this.State = ProcessDefinitionState.Active;
        //    this.Description = process.Documentation;
        //}

        public virtual int Id
        {
            get;
            protected set;
        }

        public virtual int OwnerId
        {
            get;
            protected set;
        }

        public virtual int? OrganizationId
        {
            get;
            set;
        }

        public virtual int? CategoryId
        {
            get;
            set;
        }

        public virtual byte[] Model
        {
            get => this.model?.Value;
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

        public virtual string ConcurrencyStamp
        {
            get;
            set;
        }

        public virtual ProcessDefinitionState State
        {
            get;
            protected set;
        }

        public virtual string Tag
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        //public virtual Model Model
        //{
        //    get => this.model;
        //}

        //public virtual Process GetProcessModel()
        //{
        //    if(this.process == null)
        //    {
        //        if (this.model == null)
        //            throw new Exception("The model was not initialized.");

        //        using (var stream = new MemoryStream(this.model.Content))
        //        {
        //            var parser = BpmnModelParser.Create();
        //            var bpmnModel = parser.Parse(stream);
        //            this.process = bpmnModel.FindProcessById(this.Key);

        //            if (this.process == null)
        //                throw new Exception("Invalid BPMN Process model.");
        //        }
        //    }

        //    return this.process;
        //}
    }

    public enum ProcessDefinitionState : int
    {
        Inactive = 0,

        Active = 1
    }
}
