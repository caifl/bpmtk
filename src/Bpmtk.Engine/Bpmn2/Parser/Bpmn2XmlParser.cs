using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class Bpmn2XmlParser
    {
        protected BpmnFactory factory = new BpmnFactory();
        protected Definitions definitions;

        protected static readonly XmlReaderSettings settings;
        private const string schemaLocation = "Bpmtk.Engine.Bpmn2.Schema";
        private static readonly string[] schemaNames = new string[]
            {
                "BPMN20.xsd", "BPMNDI.xsd", "DI.xsd", "DC.xsd", "Semantic.xsd", "Extensions.xsd"
            };

        static Bpmn2XmlParser()
        {
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

            var handler = new DefinitionsParseHandler();

            var context = new Bpmn2XmlParseContext(definitions, this.factory);

            this.definitions = handler.Create(null, context, element) as Definitions;

            context.Complete();

            return definitions;
        }
    }
}
