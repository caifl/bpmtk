using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Bpmtk.Bpmn2.Extensions;

namespace Bpmtk.Bpmn2.Parser
{
    public interface IBpmnHandler<TParent>
    {
        object Create(TParent parent, IParseContext context, XElement element);
    }

    abstract class BaseElementHandler<TParent, TBaseElement>
        : BpmnHandler<TParent, TBaseElement>
        where TBaseElement : BaseElement
    {
        public BaseElementHandler()
        {
            this.handlers.Add("documentation", new BpmnHandlerCallback<TBaseElement>(ParseDocumentation));
            this.handlers.Add("extensionElements", new ExtensionElementsHandler<TBaseElement>());
        }

        public override TBaseElement Create(TParent parent, IParseContext context,
            XElement element)
        {
            var baseElement = base.Create(parent, context, element);

            baseElement.Id = element.GetAttribute("id");

            return baseElement;
        }

        protected virtual Documentation ParseDocumentation(TBaseElement parent, IParseContext context,
            XElement element)
        {
            var documentation = context.BpmnFactory.CreateDocumentation();

            documentation.Text = element.Value;
            documentation.Id = element.GetAttribute("id");
            documentation.TextFormat = element.GetAttribute("textFormat");

            parent.Documentations.Add(documentation);

            return documentation;
        }

        protected virtual IList<EventListener> ParseEventListeners(ExtensionElements extensionElements)
        {
            List<EventListener> list = new List<EventListener>();

            if (extensionElements != null && extensionElements.Items.Count > 0)
            {
                string value = null;
                foreach (var item in extensionElements.Items)
                {
                    if (item.Name.LocalName != "eventListener")
                        continue;

                    var eventListener = new EventListener();
                    eventListener.Event = item.GetAttribute("event");

                    value = item.GetAttribute("class");
                    if (!string.IsNullOrEmpty(value))
                        eventListener.Class = value;

                    list.Add(eventListener);

                    var children = item.Elements();
                    foreach (var child in children)
                    {
                        if (child.Name.LocalName == "script")
                        {
                            value = child.Value;
                            if(!string.IsNullOrEmpty(value))
                                eventListener.Script = child.Value;
                            eventListener.ScriptFormat = child.GetAttribute("scriptFormat");
                            continue;
                        }
                    }
                }
            }

            return list;
        }
    }

    abstract class BpmnHandler<TParent, TItem> : IBpmnHandler<TParent>
    {
        protected readonly Dictionary<string, IBpmnHandler<TItem>> handlers = new Dictionary<string, IBpmnHandler<TItem>>();

        public virtual TItem Create(TParent parent, IParseContext context, XElement element)
        {
            var item = this.New(context, element);

            this.CreateChildren(item, context, element);

            return item;
        }

        object IBpmnHandler<TParent>.Create(TParent parent, IParseContext context, XElement element)
            => Create(parent, context, element);

        protected virtual void CreateChildren(TItem parent, IParseContext context,
            XElement element          
            )
        {
            if (!element.HasElements)
                return;

            string localName = null;
            var elements = element.Elements();
            IBpmnHandler<TItem> handler = null;

            foreach (var child in elements)
            {
                localName = child.Name.LocalName; //Helper.GetRealLocalName(child);

                if (this.handlers.TryGetValue(localName, out handler))
                    handler.Create(parent, context, child);
            }
        }

        protected abstract TItem New(IParseContext context, XElement element);
    }

    class BpmnHandlerCallback<TParent> : IBpmnHandler<TParent>
    {
        protected readonly Func<TParent, IParseContext, XElement, object> callback;

        public BpmnHandlerCallback(Func<TParent, IParseContext, XElement, object> callback)
        {
            this.callback = callback;
        }

        public object Create(TParent parent, IParseContext context, XElement element)
            => this.callback(parent, context, element);
    }
}
