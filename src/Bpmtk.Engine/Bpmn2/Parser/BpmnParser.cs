using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class BpmnParser
    {
        protected static readonly XmlSchemaSet schemaSet = new XmlSchemaSet();
        protected BpmnFactory factory = new BpmnFactory();
        protected Definitions definitions;

        protected readonly XmlReaderSettings settings;
        private const string schemaLocation = "Bpmtk.Engine.Bpmn2.Schema";
        private static readonly string[] schemaNames = new string[]
            {
                "BPMN20.xsd", "BPMNDI.xsd", "DI.xsd", "DC.xsd", "Semantic.xsd", "Extensions.xsd"
            };

        static BpmnParser()
        {
            var assembly = typeof(BpmnParser).Assembly;
            var names = assembly.GetManifestResourceNames();

            foreach (var name in schemaNames)
            {
                var resourceKey = string.Concat(schemaLocation, ".", name);
                using (var stream = assembly.GetManifestResourceStream(resourceKey))
                {
                    var schema = XmlSchema.Read(stream, null);
                    schemaSet.Add(schema);
                }
            }
        }

        protected BpmnParser()
        {
            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //_settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += OnValidationEventHandler;

            settings.Schemas.Add(schemaSet);
        }

        private List<Exception> exceptions = new List<Exception>();

        protected virtual void OnValidationEventHandler(object sender, ValidationEventArgs e)
        {
            this.exceptions.Add(e.Exception);
        }

        public static BpmnParser Create()
        {
            return new BpmnParser();
        }

        public virtual BpmnParserResults Parse(Stream stream)
        {
            this.exceptions.Clear();
            XDocument document = null;
            using (var reader = XmlReader.Create(stream, settings))
            {
                document = XDocument.Load(reader);
            }

            var element = document.Root;

            var handler = new DefinitionsParseHandler();

            var context = new BpmnParseContext(definitions, this.factory);

            this.definitions = handler.Create(null, context, element) as Definitions;

            context.Complete();

            var flowElements = context.FlowElements;
            return new BpmnParserResults(definitions, flowElements, this.exceptions);
        }
    }
}
