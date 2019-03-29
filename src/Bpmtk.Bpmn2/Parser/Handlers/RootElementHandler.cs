using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class ItemDefintionHandler : BaseElementHandler<Definitions, ItemDefinition>
    {
        public override ItemDefinition Create(Definitions parent, IParseContext context, XElement element)
        {
            var itemDef = base.Create(parent, context, element);

            itemDef.StructureRef = element.GetAttribute("structureRef");
            itemDef.IsCollection = element.GetBoolean("isCollection");
            itemDef.ItemKind = element.GetEnum<ItemKind>("itemKind", ItemKind.Information);

            parent.RootElements.Add(itemDef);

            return itemDef;
        }

        protected override ItemDefinition New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateItemDefinition();
    }

    class MessageHandler : BaseElementHandler<Definitions, Message>
    {
        public override Message Create(Definitions parent, IParseContext context, XElement element)
        {
            var message = base.Create(parent, context, element);

            message.ItemRef = element.GetAttribute("itemRef");
            message.Name = element.GetAttribute("name");

            parent.RootElements.Add(message);

            return message;
        }

        protected override Message New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateMessage();
    }

    class SignalHandler : BaseElementHandler<Definitions, Signal>
    {
        public override Signal Create(Definitions parent, IParseContext context, XElement element)
        {
            var signal = base.Create(parent, context, element);

            signal.Name = element.GetAttribute("name");
            signal.StructureRef = element.GetAttribute("structureRef");

            parent.RootElements.Add(signal);

            return signal;
        }

        protected override Signal New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateSignal();
    }

    class ErrorHandler : BaseElementHandler<Definitions, Error>
    {
        public override Error Create(Definitions parent, IParseContext context, XElement element)
        {
            var error = base.Create(parent, context, element);

            error.Name = element.GetAttribute("name");
            error.StructureRef = element.GetAttribute("structureRef");

            parent.RootElements.Add(error);

            return error;
        }

        protected override Error New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateError();
    }

    class DataStoreHandler : BaseElementHandler<Definitions, DataStore>
    {
        public DataStoreHandler()
        {
            //this.handlers.Add("dataState")
        }

        public override DataStore Create(Definitions parent, IParseContext context, XElement element)
        {
            var dataStore = base.Create(parent, context, element);

            dataStore.Name = element.GetAttribute("name");

            var value = element.GetAttribute("capacity");
            if (!string.IsNullOrEmpty(value))
                dataStore.Capacity = Convert.ToInt32(value);

            value = element.GetAttribute("isUnlimited");
            if (!string.IsNullOrEmpty(value))
                dataStore.IsUnlimited = Convert.ToBoolean(value);

            dataStore.ItemSubjectRef = element.GetAttribute("structureRef");

            parent.RootElements.Add(dataStore);

            return dataStore;
        }

        protected override DataStore New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataStore();
    }

    class InterfaceHandler : BaseElementHandler<Definitions, Interface>
    {
        public InterfaceHandler()
        {
            this.handlers.Add("operation", new OperationHandler());
        }

        public override Interface Create(Definitions parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);

            item.ImplementationRef = element.GetAttribute("implementationRef");
            item.Name = element.GetAttribute("name");

            parent.RootElements.Add(item);

            return item;
        }

        protected override Interface New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateInterface();
    }

    class OperationHandler : BaseElementHandler<Interface, Operation>
    {
        public OperationHandler()
        {
            this.handlers.Add("inMessageRef", new BpmnHandlerCallback<Operation>((p, c, e) =>
            {
                return p.InMessageRef = e.Value;
            }));

            this.handlers.Add("outMessageRef", new BpmnHandlerCallback<Operation>((p, c, e) =>
            {
                return p.OutMessageRef = e.Value;
            }));

            this.handlers.Add("errorRef", new BpmnHandlerCallback<Operation>((p, c, e) =>
            {
                var errorRef = e.Value;
                p.ErrorRefs.Add(errorRef);

                return errorRef;
            }));
        }

        public override Operation Create(Interface parent, IParseContext context, XElement element)
        {
            var op = base.Create(parent, context, element);
            op.Interface = parent;

            op.Name = element.GetAttribute("name");
            op.ImplementationRef = element.GetAttribute("implementationRef");

            parent.Operations.Add(op);

            return op;
        }

        protected override Operation New(IParseContext context, XElement element)
         => context.BpmnFactory.CreateOperation();
    }

    class ImportHandler : BaseElementHandler<Definitions, Import>
    {
        public override Import Create(Definitions parent, IParseContext context, XElement element)
        {
            var import = base.Create(parent, context, element);

            import.Id = element.GetAttribute("id");
            import.Namespace = element.GetAttribute("namespace");
            import.Location = element.GetAttribute("location");
            import.ImportType = element.GetAttribute("importType");

            parent.Imports.Add(import);

            return import;
        }

        protected override Import New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateImport();
    }
}
