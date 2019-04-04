using System;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class SequenceFlowParseHandler : FlowElementParseHandler
    {
        public SequenceFlowParseHandler()
        {
            this.handlers.Add("conditionExpression", new ExpressionParseHandler<SequenceFlow>((parent, result) =>
            {
                parent.ConditionExpression = result;
            }));
        }

        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var sequenceFlow = context.BpmnFactory.CreateSequenceFlow();
            parent.FlowElements.Add(sequenceFlow);

            base.Init(sequenceFlow, context, element);

            var sourceRef = element.GetAttribute("sourceRef");
            var targetRef = element.GetAttribute("targetRef");

            context.AddReferenceRequest(sourceRef, (FlowNode node) => sequenceFlow.SourceRef = node);
            context.AddReferenceRequest(targetRef, (FlowNode node) => sequenceFlow.TargetRef = node);

            context.AddSourceRef(sourceRef, sequenceFlow);
            context.AddTargetRef(targetRef, sequenceFlow);

            return sequenceFlow;
        }
    }
}
