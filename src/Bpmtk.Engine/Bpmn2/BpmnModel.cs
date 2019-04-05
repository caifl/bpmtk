using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Bpmn2.Parser;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnModel
    {
        private readonly Definitions definitions;
        private readonly byte[] modelData;
        private readonly IEnumerable<Process> processes;
        private readonly IDictionary<string, Process> processById;
        private readonly IReadOnlyDictionary<string, FlowElement> flowElementById;

        protected BpmnModel(Definitions definitions, IReadOnlyDictionary<string, FlowElement> flowElements,
            byte[] modelData)
        {
            this.definitions = definitions;
            this.modelData = modelData;
            this.processes = definitions.RootElements.OfType<Process>();
            this.processById = this.processes.ToDictionary(x => x.Id);
            this.flowElementById = flowElements;
        }

        public virtual byte[] Data => this.modelData;

        public static BpmnModel FromBytes(byte[] bytes, bool disableValidation = false)
        {
            var parser = BpmnParser.Create();
            BpmnParserResults results = null;
            using (var stream = new MemoryStream(bytes))
            {
                results = parser.Parse(stream);
            }

            return new BpmnModel(results.Definitions, results.FlowElements, bytes);
        }

        public virtual IEnumerable<Process> Processes
        {
            get => this.processes;
        }

        public virtual bool HasDiagram(string processId)
        {
            return definitions.Diagrams.Any(x => x.BPMNPlane.BpmnElement == processId);
        }

        public virtual Process GetProcess(string id)
        {
            Process value = null;
            this.processById.TryGetValue(id, out value);

            return value;
        }

        public virtual FlowElement GetFlowElement(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            FlowElement element = null;
            if (this.flowElementById.TryGetValue(id, out element))
                return element;

            return null;
        }
    }
}
