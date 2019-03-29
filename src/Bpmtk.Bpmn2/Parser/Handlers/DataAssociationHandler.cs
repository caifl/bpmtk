using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class DataInputAssociationHandler<TParent> : BaseElementHandler<TParent, DataInputAssociation>
        where TParent : IHasDataInputAssociations
    {
        public DataInputAssociationHandler()
        {
            this.handlers.Add("sourceRef", new BpmnHandlerCallback<DataInputAssociation>((p, callback, x) =>
            {
                p.SourceRefs.Add(x.Value);

                return x.Value;
            }));

            this.handlers.Add("targetRef", new BpmnHandlerCallback<DataInputAssociation>((p, callback, x) =>
            {
                return p.TargetRef = x.Value;
            }));

            this.handlers.Add("assignment", new AssignmentHandler<DataInputAssociation>());
            this.handlers.Add("transformation", new ExpressionHandler<DataInputAssociation>((p, callback, x, result) =>
            {
                p.Transformation = result as FormalExpression;
            }));
        }

        public override DataInputAssociation Create(TParent parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            parent.DataInputAssociations.Add(item);

            return item;
        }

        protected override DataInputAssociation New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataInputAssociation();
    }

    class DataOutputAssociationHandler<TParent> : BaseElementHandler<TParent, DataOutputAssociation>
        where TParent : IHasDataOutputAssociations
    {
        public DataOutputAssociationHandler()
        {
            this.handlers.Add("sourceRef", new BpmnHandlerCallback<DataOutputAssociation>((p, callback, x) =>
            {
                p.SourceRefs.Add(x.Value);

                return x.Value;
            }));

            this.handlers.Add("targetRef", new BpmnHandlerCallback<DataOutputAssociation>((p, callback, x) =>
            {
                return p.TargetRef = x.Value;
            }));
            
            this.handlers.Add("assignment", new AssignmentHandler<DataOutputAssociation>());
            this.handlers.Add("transformation", new ExpressionHandler<DataOutputAssociation>((p, callback, x, result) =>
            {
                p.Transformation = result as FormalExpression;
            }));
        }

        public override DataOutputAssociation Create(TParent parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            parent.DataOutputAssociations.Add(item);

            return item;
        }

        protected override DataOutputAssociation New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateDataOutputAssociation();
    }

    class AssignmentHandler<TParent> : BaseElementHandler<TParent, Assignment>
        where TParent : DataAssociation
    {
        public AssignmentHandler()
        {
            this.handlers.Add("from", new ExpressionHandler<Assignment>((p, callback, x, result) =>
            {
                p.From = result;
            }));

            this.handlers.Add("to", new ExpressionHandler<Assignment>((p, callback, x, result) =>
            {
                p.To = result;
            }));
        }

        public override Assignment Create(TParent parent, IParseContext context, XElement element)
        {
            var item = base.Create(parent, context, element);

            parent.Assignments.Add(item);

            return item;
        }

        protected override Assignment New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateAssignment();
    }
}
