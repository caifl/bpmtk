using System;
using System.Xml.Linq;
using Bpmtk.Engine.Bpmn2.Parser.Handlers;

namespace Bpmtk.Engine.Bpmn2.Parser
{
    public abstract class BaseElementParseHandler : ParseHandler
    {
        protected BaseElementParseHandler()
        {
            this.handlers.Add("documentation", new ParseHandlerAction<BaseElement>((p, c, x) => {
                this.ParseDocumentation(p, c, x);
            }));

            this.handlers.Add("extensionElements", new ParseHandlerAction<BaseElement>((p, c, x) => {
                this.ParseExtensionElements(p, c, x);
            }));
        }

        protected virtual void Init(BaseElement baseElement,
            IParseContext context, XElement element)
        {
            baseElement.Id = element.GetAttribute("id");

            if (element.HasElements)
                this.CreateChildren(baseElement, context, element);
        }

        protected virtual Documentation ParseDocumentation(BaseElement parent, IParseContext context,
            XElement element)
        {
            var documentation = context.BpmnFactory.CreateDocumentation();

            documentation.Text = element.Value;
            documentation.Id = element.GetAttribute("id");
            documentation.TextFormat = element.GetAttribute("textFormat");

            parent.Documentations.Add(documentation);

            return documentation;
        }

        protected virtual ExtensionElements ParseExtensionElements(BaseElement parent,
            IParseContext context, XElement element)
        {
            return null;
        }

        //protected virtual IList<EventListener> ParseEventListeners(ExtensionElements extensionElements)
        //{
        //    List<EventListener> list = new List<EventListener>();

        //    if (extensionElements != null && extensionElements.Items.Count > 0)
        //    {
        //        string value = null;
        //        foreach (var item in extensionElements.Items)
        //        {
        //            if (item.Name.LocalName != "eventListener")
        //                continue;

        //            var eventListener = new EventListener();
        //            eventListener.Event = item.GetAttribute("event");

        //            value = item.GetAttribute("class");
        //            if (!string.IsNullOrEmpty(value))
        //                eventListener.Class = value;

        //            list.Add(eventListener);

        //            var children = item.Elements();
        //            foreach (var child in children)
        //            {
        //                if (child.Name.LocalName == "script")
        //                {
        //                    value = child.Value;
        //                    if (!string.IsNullOrEmpty(value))
        //                        eventListener.Script = child.Value;
        //                    eventListener.ScriptFormat = child.GetAttribute("scriptFormat");
        //                    continue;
        //                }
        //            }
        //        }
        //    }

        //    return list;
        //}
    }

    public abstract class BaseElementParseHandler<TParent> : ParseHandler<TParent>
    {
        protected BaseElementParseHandler()
        {
            this.handlers.Add("documentation", new ParseHandlerAction<BaseElement>((p, c, x) => {
                this.ParseDocumentation(p, c, x);
            }));

            this.handlers.Add("extensionElements", new ParseHandlerAction<BaseElement>((p, c, x) => {
                this.ParseExtensionElements(p, c, x);
            }));
        }

        protected virtual void Init(BaseElement baseElement,
            IParseContext context, XElement element)
        {
            baseElement.Id = element.GetAttribute("id");

            if (element.HasElements)
                this.CreateChildren(baseElement, context, element);
        }

        protected virtual Documentation ParseDocumentation(BaseElement parent, IParseContext context,
            XElement element)
        {
            var documentation = context.BpmnFactory.CreateDocumentation();

            documentation.Text = element.Value;
            documentation.Id = element.GetAttribute("id");
            documentation.TextFormat = element.GetAttribute("textFormat");

            parent.Documentations.Add(documentation);

            return documentation;
        }

        protected virtual ExtensionElements ParseExtensionElements(BaseElement parent,
            IParseContext context, XElement element)
        {
            return null;
        }
    }
}
