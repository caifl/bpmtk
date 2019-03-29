using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ExtensionElementsHandler<TParent> : BpmnHandler<TParent, ExtensionElements>
        where TParent : BaseElement
    {
        public override ExtensionElements Create(TParent parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);

            if (element.HasElements)
            {
                var children = element.Elements();
                foreach (var child in children)
                    item.Items.Add(child);
            }

            parent.ExtensionElements = item;

            return item;
        }

        protected override ExtensionElements New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateExtensionElements();
    }
}
