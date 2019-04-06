using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class ExtensionElementsParseHandler : ParseHandler<BaseElement>
    {
        public override object Create(BaseElement parent, IParseContext context, XElement element)
        {
            var extensionElements = context.BpmnFactory.CreateExtensionElements();
            parent.ExtensionElements = extensionElements;

            return extensionElements;
        }
    }
}
