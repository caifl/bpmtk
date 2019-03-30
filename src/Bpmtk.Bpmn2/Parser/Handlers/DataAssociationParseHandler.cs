using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class DataInputAssociationParseHandler : BaseElementParseHandler
    {
        public DataInputAssociationParseHandler()
        {
            //this.handlers.Add("sourceRef", new BpmnHandlerCallback<DataInputAssociation>((p, callback, x) =>
            //{
            //    //p.SourceRefs.Add(x.Value);

            //    return x.Value;
            //}));

            //this.handlers.Add("targetRef", new BpmnHandlerCallback<DataInputAssociation>((p, callback, x) =>
            //{
            //    return p.TargetRef = x.Value;
            //}));

            this.handlers.Add("assignment", new AssignmentParseHandler());
            this.handlers.Add("transformation", new ExpressionParseHandler<DataInputAssociation>((p, result) =>
            {
                p.Transformation = result as FormalExpression;
            }));
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateDataInputAssociation();
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            //parent.DataInputAssociations.Add(item);

            return item;
        }
    }

    class DataOutputAssociationParseHandler : BaseElementParseHandler
    {
        public DataOutputAssociationParseHandler()
        {
            //this.handlers.Add("sourceRef", new BpmnHandlerCallback<DataOutputAssociation>((p, callback, x) =>
            //{
            //    p.SourceRefs.Add(x.Value);

            //    return x.Value;
            //}));

            //this.handlers.Add("targetRef", new BpmnHandlerCallback<DataOutputAssociation>((p, callback, x) =>
            //{
            //    return p.TargetRef = x.Value;
            //}));

            this.handlers.Add("assignment", new AssignmentParseHandler());
            this.handlers.Add("transformation", new ExpressionParseHandler<DataOutputAssociation>((p, result) =>
            {
                p.Transformation = result as FormalExpression;
            }));
        }

        public override object Create(object parent, IParseContext context, XElement element)
        {
            var item = context.BpmnFactory.CreateDataOutputAssociation();
            //item.TargetRef = element.GetAttribute("targetRef");
            //item.

            //parent.DataOutputAssociations.Add(item);

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
