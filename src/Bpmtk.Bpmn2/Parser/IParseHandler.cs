using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    public interface IParseHandler
    {
        object Create(object parent, IParseContext context, XElement element);
    }

    public class ParseHandlerAction<TParent> : IParseHandler
    {
        private readonly Action<TParent, IParseContext, XElement> callback;

        public ParseHandlerAction(Action<TParent, IParseContext, XElement> callback)
        {
            this.callback = callback;
        }

        public virtual object Create(object parent, IParseContext context, XElement element)
        {
            this.callback((TParent)parent, context, element);

            return element;
        }
    }

    public abstract class ParseHandler<TParent> : IParseHandler
    {
        protected readonly Dictionary<string, IParseHandler> handlers = new Dictionary<string, IParseHandler>();

        public abstract object Create(TParent parent, IParseContext context, XElement element);

        protected virtual void CreateChildren(object parent, IParseContext context, XElement element)
        {
            var children = element.Elements();
            string localName = null;
            IParseHandler handler = null;

            foreach (var child in children)
            {
                localName = child.Name.LocalName;

                if (this.handlers.TryGetValue(localName, out handler))
                    handler.Create(parent, context, child);
            }
        }

        object IParseHandler.Create(object parent, IParseContext context, XElement element)
            => this.Create((TParent)parent, context, element);
    }

    public abstract class ParseHandler : IParseHandler
    {
        protected readonly Dictionary<string, IParseHandler> handlers = new Dictionary<string, IParseHandler>();

        public abstract object Create(object parent, IParseContext context, XElement element);

        protected virtual void CreateChildren(object parent, IParseContext context, XElement element)
        {
            var children = element.Elements();
            string localName = null;
            IParseHandler handler = null;

            foreach (var child in children)
            {
                localName = child.Name.LocalName;

                if (this.handlers.TryGetValue(localName, out handler))
                    handler.Create(parent, context, child);
            }
        }
    }
}
