using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Bpmtk.Bpmn2.Parser
{
    public class Bpmn2XmlParser
    {
        protected BpmnFactory factory = new BpmnFactory();
        protected Definitions definitions;

        protected static readonly XmlReaderSettings settings;
        private const string schemaLocation = "Bpmtk.Bpmn2.Schema";
        private static readonly string[] schemaNames = new string[]
            {
                "BPMN20.xsd", "BPMNDI.xsd", "DI.xsd", "DC.xsd", "Semantic.xsd", "Extensions.xsd"
            };

        protected static readonly Dictionary<string, IBpmnHandler<Definitions>> definitionsHandlers = new Dictionary<string, IBpmnHandler<Definitions>>();

        static Bpmn2XmlParser()
        {
            definitionsHandlers.Add("resource", new ResourceHandler());
            definitionsHandlers.Add("process", new ProcessHandler());
            definitionsHandlers.Add("itemDefinition", new ItemDefintionHandler());
            definitionsHandlers.Add("message", new MessageHandler());
            definitionsHandlers.Add("signal", new SignalHandler());
            definitionsHandlers.Add("interface", new InterfaceHandler());
            definitionsHandlers.Add("error", new ErrorHandler());
            definitionsHandlers.Add("dataStore", new DataStoreHandler());
            definitionsHandlers.Add("BPMNDiagram", new BpmnDiagramHandler());

            //eventDefinitions
            var keys = EventDefinitionHandler.Keys;
            var eventDefinitionHandler = new EventDefinitionHandler();
            foreach (var key in keys)
                definitionsHandlers.Add(key, eventDefinitionHandler);

            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //_settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += Settings_ValidationEventHandler; ;

            var assembly = typeof(Bpmn2XmlParser).Assembly;
            var names = assembly.GetManifestResourceNames();

            var schemaSet = new XmlSchemaSet();

            foreach (var name in schemaNames)
            {
                var resourceKey = string.Concat(schemaLocation, ".", name);
                using (var stream = assembly.GetManifestResourceStream(resourceKey))
                {
                    var schema = XmlSchema.Read(stream, null);
                    schemaSet.Add(schema);
                }
            }

            settings.Schemas.Add(schemaSet);
        }

        private static void Settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
        }

        public static Bpmn2XmlParser Create()
        {
            return new Bpmn2XmlParser();
        }

        public virtual Definitions Parse(Stream stream)
        {
            var reader = XmlReader.Create(stream, settings);

            XDocument document = XDocument.Load(reader);
            var element = document.Root;

            this.definitions = this.ParseDefinitions(element);

            return definitions;
        }

        protected virtual Definitions ParseDefinitions(XElement element)
        {
            var definitions = this.factory.CreateDefinitions();

            definitions.Id = element.GetAttribute("id");
            definitions.Name = element.GetAttribute("name");
            definitions.Exporter = element.GetAttribute("exporter");
            definitions.ExporterVersion = element.GetAttribute("exporterVersion");
            definitions.ExpressionLanguage = element.GetAttribute("expressionLanguage");
            definitions.TypeLanguage = element.GetAttribute("typeLanguage");
            definitions.TargetNamespace = element.GetAttribute("targetNamespace");

            var context = new Bpmn2XmlParseContext(definitions, this.factory);

            this.ParseChildren(element, definitions, context);

            return definitions;
        }

        protected virtual void ParseChildren(XElement element,
            Definitions parent,
            IParseContext context)
        {
            if (!element.HasElements)
                return;

            string localName = null;
            var elements = element.Elements();
            IBpmnHandler<Definitions> handler = null;

            foreach (var child in elements)
            {
                localName = child.Name.LocalName;
                if (definitionsHandlers.TryGetValue(localName, out handler))
                    handler.Create(parent, context, child);
            }
        }
    }
}
