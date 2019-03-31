using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
{
    class ItemDefintionParseHandler : BaseElementParseHandler<Definitions>
    {
        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateItemDefinition();
            parent.RootElements.Add(item);

            item.StructureRef = element.GetAttribute("structureRef");
            item.IsCollection = element.GetBoolean("isCollection");
            item.ItemKind = element.GetEnum("itemKind", ItemKind.Information);

            base.Init(item, context, element);

            context.Push(item);

            return item;
        }
    }

    class MessageParseHandler : BaseElementParseHandler<Definitions>
    {
        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var message = context.BpmnFactory.CreateMessage();
            message.Name = element.GetAttribute("name");

            parent.RootElements.Add(message);

            var itemRef = element.GetAttribute("itemRef");
            if(itemRef != null)
                context.AddReferenceRequest(itemRef, (ItemDefinition x) => message.ItemRef = x);

            base.Init(message, context, element);

            context.Push(message);

            return message;
        }
    }

    class SignalParseHandler : BaseElementParseHandler<Definitions>
    {
        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var signal = context.BpmnFactory.CreateSignal();

            signal.Name = element.GetAttribute("name");
            parent.RootElements.Add(signal);

            var structureRef = element.GetAttribute("structureRef");
            if (structureRef != null)
                context.AddReferenceRequest(structureRef, (ItemDefinition x) => signal.StructureRef = x);

            base.Init(signal, context, element);

            context.Push(signal);

            return signal;
        }
    }

    class ErrorParseHandler : BaseElementParseHandler<Definitions>
    {
        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var error = context.BpmnFactory.CreateError();

            error.Name = element.GetAttribute("name");
            parent.RootElements.Add(error);

            var structureRef = element.GetAttribute("structureRef");
            if(structureRef != null)
                context.AddReferenceRequest<ItemDefinition>(structureRef, x => error.StructureRef = x);

            base.Init(error, context, element);

            context.Push(error);

            return error;
        }
    }

    class DataStoreParseHandler : BaseElementParseHandler<Definitions>
    {
        public DataStoreParseHandler()
        {
            //this.handlers.Add("dataState")
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var dataStore = context.BpmnFactory.CreateDataStore();
            parent.RootElements.Add(dataStore);

            dataStore.Name = element.GetAttribute("name");

            var value = element.GetAttribute("capacity");
            if (!string.IsNullOrEmpty(value))
                dataStore.Capacity = Convert.ToInt32(value);

            value = element.GetAttribute("isUnlimited");
            if (!string.IsNullOrEmpty(value))
                dataStore.IsUnlimited = Convert.ToBoolean(value);

            var itemSubjectRef = element.GetAttribute("itemSubjectRef");
            if(itemSubjectRef != null)
                context.AddReferenceRequest(itemSubjectRef, (ItemDefinition target) => dataStore.ItemSubjectRef = target);

            base.Init(dataStore, context, element);

            return dataStore;
        }
    }

    class InterfaceParseHandler : BaseElementParseHandler<Definitions>
    {
        public InterfaceParseHandler()
        {
            this.handlers.Add("operation", new OperationParseHandler());
        }

        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateInterface();
            parent.RootElements.Add(item);

            item.ImplementationRef = element.GetAttribute("implementationRef");
            item.Name = element.GetAttribute("name");

            base.Init(item, context, element);

            context.Push(item);

            return item;
        }
    }

    class OperationParseHandler : BaseElementParseHandler<Interface>
    {
        public OperationParseHandler()
        {
            this.handlers.Add("inMessageRef", new ParseHandlerAction<Operation>((p, c, e) =>
            {
                var inMessageRef = e.Value;
                if (inMessageRef != null)
                    c.AddReferenceRequest<Message>(inMessageRef, (x) => p.InMessageRef = x);
            }));

            this.handlers.Add("outMessageRef", new ParseHandlerAction<Operation>((p, c, e) =>
            {
                var outMessageRef = e.Value;
                if (outMessageRef != null)
                    c.AddReferenceRequest<Message>(outMessageRef, (x) => p.OutMessageRef = x);
            }));

            this.handlers.Add("errorRef", new ParseHandlerAction<Operation>((p, c, e) =>
            {
                var errorRef = e.Value;
                if (errorRef != null)
                    c.AddReferenceRequest<Error>(errorRef, (x) => p.ErrorRefs.Add(x));
            }));
        }

        public override object Create(Interface parent, IParseContext context, XElement element)
        {
            var op = context.BpmnFactory.CreateOperation();
            parent.Operations.Add(op);

            op.Interface = parent;

            op.Name = element.GetAttribute("name");
            op.ImplementationRef = element.GetAttribute("implementationRef");

            base.Init(op, context, element);

            context.Push(op);

            return op;
        }
    }

    class ImportParseHandler : BaseElementParseHandler<Definitions>
    {
        public override object Create(Definitions parent, IParseContext context, XElement element)
        {
            var import = context.BpmnFactory.CreateImport();
            parent.Imports.Add(import);

            import.Id = element.GetAttribute("id");
            import.Namespace = element.GetAttribute("namespace");
            import.Location = element.GetAttribute("location");
            import.ImportType = element.GetAttribute("importType");

            base.Init(import, context, element);

            return import;
        }
    }
}
