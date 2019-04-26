using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Bpmtk.Bpmn2;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public class BpmnParserResults
    {
        public BpmnParserResults(Definitions definitions,
            IDictionary<string, FlowElement> flowElements,
            IEnumerable<Exception> exceptions)
        {
            this.Definitions = definitions;
            this.FlowElements = new ReadOnlyDictionary<string, FlowElement>(flowElements);
            this.Exceptions = new ReadOnlyCollection<Exception>(exceptions.ToList());
        }

        public virtual IReadOnlyList<Exception> Exceptions
        {
            get;
        }

        public virtual IReadOnlyDictionary<string, FlowElement> FlowElements
        {
            get;
        }

        public virtual Definitions Definitions
        {
            get;
        }
    }
}
