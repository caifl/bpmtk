using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    public abstract class BaseElementParseHandler : ParseHandler
    {
        protected virtual void Init(BaseElement baseElement,
            IParseContext context, XElement element)
        {
            baseElement.Id = element.GetAttribute("id");

            if (element.HasElements)
                this.CreateChildren(baseElement, context, element);
        }
    }

    public abstract class BaseElementParseHandler<TParent> : ParseHandler<TParent>
    {
        protected virtual void Init(BaseElement baseElement,
            IParseContext context, XElement element)
        {
            baseElement.Id = element.GetAttribute("id");

            if (element.HasElements)
                this.CreateChildren(baseElement, context, element);
        }
    }
}
