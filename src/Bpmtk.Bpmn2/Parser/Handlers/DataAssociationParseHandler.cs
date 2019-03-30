using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    abstract class DataAssociationParseHandler : BaseElementParseHandler
    {
        public DataAssociationParseHandler()
        {
            this.handlers.Add("sourceRef", new ParseHandlerAction<DataInputAssociation>((p, ctx, x) =>
            {
                var sourceRef = x.Value;
                if (sourceRef != null)
                    ctx.AddReferenceRequest<IItemAwareElement>(sourceRef, r => p.SourceRefs.Add(r));
            }));

            this.handlers.Add("targetRef", new ParseHandlerAction<DataInputAssociation>((p, ctx, x) =>
            {
                var targetRef = x.Value;
                if (targetRef != null)
                    ctx.AddReferenceRequest<IItemAwareElement>(targetRef, r => p.TargetRef = r);
            }));

            this.handlers.Add("assignment", new AssignmentParseHandler());
            this.handlers.Add("transformation", new ExpressionParseHandler<DataInputAssociation>((p, result) =>
            {
                p.Transformation = result as FormalExpression;
            }));
        }
    }

    class DataInputAssociationParseHandler<TParent> : DataAssociationParseHandler
    {
        private readonly Action<TParent, DataInputAssociation> callback;

        public DataInputAssociationParseHandler(Action<TParent, DataInputAssociation> callback)
        {
            this.callback = callback;
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateDataInputAssociation();
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            //parent.DataInputAssociations.Add(item);
            if (this.callback != null)
                this.callback((TParent)parent, item);

            return item;
        }
    }

    class DataOutputAssociationParseHandler<TParent> : DataAssociationParseHandler
    {
        private readonly Action<TParent, DataOutputAssociation> callback;

        public DataOutputAssociationParseHandler(Action<TParent, DataOutputAssociation> callback)
        {
            this.callback = callback;
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateDataOutputAssociation();
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            //parent.DataOutputAssociations.Add(item);
            if (this.callback != null)
                this.callback((TParent)parent, item);

            return item;
        }
    }

    class AssignmentParseHandler : BaseElementParseHandler<DataAssociation>
    {
        public AssignmentParseHandler()
        {
            this.handlers.Add("from", new ExpressionParseHandler<Assignment>((p, result) =>
            {
                p.From = result;
            }));

            this.handlers.Add("to", new ExpressionParseHandler<Assignment>((p, result) =>
            {
                p.To = result;
            }));
        }

        public override object Create(DataAssociation parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateAssignment();
            parent.Assignments.Add(item);

            base.Init(item, context, element);

            return item;
        }
    }
}
