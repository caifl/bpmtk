using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class BpmnParser
    {
        protected BpmnFactory factory = new BpmnFactory();
        protected Definitions definitions;

        protected static readonly XmlReaderSettings settings;
        private const string schemaLocation = "Bpmtk.Engine.Bpmn2.Schema";
        private static readonly string[] schemaNames = new string[]
            {
                "BPMN20.xsd", "BPMNDI.xsd", "DI.xsd", "DC.xsd", "Semantic.xsd", "Extensions.xsd"
            };

        static BpmnParser()
        {
            settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //_settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += Settings_ValidationEventHandler; ;

            var assembly = typeof(BpmnParser).Assembly;
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

        public static BpmnParser Create()
        {
            return new BpmnParser();
        }

        public virtual BpmnParserResults Parse(Stream stream)
        {
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

            return new BpmnParserResults(definitions, flowElements, new List<Exception>());
        }
    }
}
