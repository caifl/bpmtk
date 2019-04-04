using System;
using System.Collections.Generic;
using System.Linq;
using Bpmtk.Engine.Bpmn2.DI;

namespace Bpmtk.Engine.Bpmn2
{
    /// <summary>
    /// BPMN 2.0 document root.
    /// </summary>
    public class Definitions
    {
        public const string DefaultExpressionLanguage = "http://www.w3.org/1999/XPath";
        public const string DefaultTypeLanguage = "http://www.w3.org/2001/XMLSchema";

        public const string BPMN20_NS = "http://www.omg.org/spec/BPMN/20100524/MODEL";
        public const string BPMTK_NS = "http://www.bpmtk.org/bpmn/extensions";

        protected List<Import> imports = new List<Import>();
        protected List<RootElement> rootElements = new List<RootElement>();
        protected List<BPMNDiagram> diagrams = new List<BPMNDiagram>();

        private IDictionary<string, RootElement> rootElementById;

        public virtual RootElement GetRootElementById(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (this.rootElementById == null)
                this.rootElementById = this.rootElements.ToDictionary(x => x.Id);

            RootElement element = null;
            this.rootElementById.TryGetValue(id, out element);

            return element;
        }

        /// <remarks/>
        //[XmlElement("extension", Order = 1)]
        //public ICollection<Extension> Extensions
        //{
        //    get;
        //}

        public virtual IList<RootElement> RootElements => this.rootElements;

        public virtual IList<Import> Imports => this.imports;

        public virtual IList<BPMNDiagram> Diagrams => this.diagrams;

        public virtual string Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string TargetNamespace
        {
            get;
            set;
        }

        public virtual string ExpressionLanguage
        {
            get;
            set;
        }

        public virtual string TypeLanguage
        {
            get;
            set;
        }

        public virtual string Exporter
        {
            get;
            set;
        }

        public virtual string ExporterVersion
        {
            get;
            set;
        }

        public virtual TRootElement GetRootElementById<TRootElement>(string id)
            where TRootElement : RootElement
        {
            var element = this.GetRootElementById(id);
            if (element == null)
                return null;

            return (TRootElement)element;
        }

        public virtual Process FindProcessById(string id)
        {
            return this.rootElements.OfType<Process>().Where(x => x.Id == id).SingleOrDefault();
        }
    }
}
