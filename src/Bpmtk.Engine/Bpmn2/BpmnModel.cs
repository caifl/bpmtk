using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Bpmtk.Engine.Bpmn2;
using Bpmtk.Engine.Bpmn2.Parser;
using Bpmtk.Engine.Bpmn2.Types;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnModel
    {
        private BpmnTypes types = null;
        private readonly Dictionary<string, ITypeHandler> typeHandlers = new Dictionary<string, ITypeHandler>();
        private readonly Definitions definitions;
        private readonly byte[] modelData;
        private readonly IEnumerable<Process> processes;
        private readonly IDictionary<string, Process> processById;
        private readonly IReadOnlyDictionary<string, FlowElement> flowElementById;
        private readonly Dictionary<string, IList<ValuedDataObject>> contextSignatures = new Dictionary<string, IList<ValuedDataObject>>();

        protected BpmnModel(Definitions definitions, IReadOnlyDictionary<string, FlowElement> flowElements,
            byte[] modelData)
        {
            this.definitions = definitions;
            this.modelData = modelData;
            this.processes = definitions.RootElements.OfType<Process>();
            this.processById = this.processes.ToDictionary(x => x.Id);
            this.flowElementById = flowElements;

            if (definitions.TypeLanguage == "http://www.w3.org/2001/XMLSchema")
                this.types = XsdTypes.Instance;
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

        public virtual IList<ValuedDataObject> GetProcessDataObjects(string id)
        {
            if (!processById.ContainsKey(id))
                throw new KeyNotFoundException(nameof(id));

            IList<ValuedDataObject> items = null;

            if(!this.contextSignatures.TryGetValue(id, out items))
            {
                var process = this.processById[id];
                var dataObjects = process.FlowElements.OfType<ValuedDataObject>();

                var list = new List<ValuedDataObject>();
                foreach(var dataObject in dataObjects)
                {
                    dataObject.Initialize(this.types);
                    //var typeName = dataObject.TypeName;
                    //if (dataObject.ItemSubjectRef != null)
                    //    typeName = item.ItemSubjectRef.StructureRef;
                    list.Add(dataObject);
                }

                this.contextSignatures.Add(id, list);

                items = list;
            }

            return items;
        }

        public virtual IList<ValuedDataObject> GetSubProcessDataObjects(string id)
        {
            if (!flowElementById.ContainsKey(id))
                throw new KeyNotFoundException(nameof(id));

            IList<ValuedDataObject> items = null;
            var subProcess = this.flowElementById[id] as SubProcess;

            if (!this.contextSignatures.TryGetValue(id, out items))
            {
                var dataObjects = subProcess.FlowElements.OfType<ValuedDataObject>();

                var list = new List<ValuedDataObject>();
                foreach (var dataObject in dataObjects)
                {
                    dataObject.Initialize(this.types);
                    //var typeName = dataObject.TypeName;
                    //if (dataObject.ItemSubjectRef != null)
                    //    typeName = item.ItemSubjectRef.StructureRef;
                    list.Add(dataObject);
                }

                this.contextSignatures.Add(id, list);

                items = list;
            }

            return items;
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
