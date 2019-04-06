using System;
using System.Xml.Linq;
using Bpmtk.Engine.Bpmn2.Parser.Handlers;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    class DefinitionsParseHandler : ParseHandler
    {
        public DefinitionsParseHandler()
        {
            this.handlers.Add("process", new ProcessParseHandler());
            this.handlers.Add("resource", new ResourceParseHandler());
            this.handlers.Add("itemDefinition", new ItemDefintionParseHandler());
            this.handlers.Add("message", new MessageParseHandler());
            this.handlers.Add("signal", new SignalParseHandler());
            this.handlers.Add("interface", new InterfaceParseHandler());
            this.handlers.Add("error", new ErrorParseHandler());
            this.handlers.Add("dataStore", new DataStoreParseHandler());
            this.handlers.Add("import", new ImportParseHandler());
            this.handlers.Add("BPMNDiagram", new BPMNDiagramParseHandler());

            //eventDefinitions
            var keys = EventDefinitionParseHandler.Keys;
            var eventDefinitionHandler = new EventDefinitionParseHandler((parent, context, element, result) =>
            {
                ((Definitions)parent).RootElements.Add(result);
            });

            foreach (var key in keys)
                this.handlers.Add(key, eventDefinitionHandler);
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var definitions = context.BpmnFactory.CreateDefinitions();

            definitions.Id = element.GetAttribute("id");
            definitions.Name = element.GetAttribute("name");
            definitions.Exporter = element.GetAttribute("exporter");
            definitions.ExporterVersion = element.GetAttribute("exporterVersion");
            definitions.ExpressionLanguage = element.GetAttribute("expressionLanguage");
            definitions.TypeLanguage = element.GetAttribute("typeLanguage");
            definitions.TargetNamespace = element.GetAttribute("targetNamespace");

            if(element.HasElements)
                base.CreateChildren(definitions, context, element);

            return definitions;
        }
    }
}
