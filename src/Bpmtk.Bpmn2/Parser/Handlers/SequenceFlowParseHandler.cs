using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser.Handlers
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

            var sourceRef = element.GetAttribute("sourceRef");
            var targetRef = element.GetAttribute("targetRef");

            context.AddReferenceRequest(sourceRef, (FlowNode node) => sequenceFlow.SourceRef = node);
            context.AddReferenceRequest(targetRef, (FlowNode node) => sequenceFlow.TargetRef = node);

            context.Push(sequenceFlow);

            base.Init((BaseElement)sequenceFlow, context, element);

            //if (sequenceFlow.ExtensionElements != null)
            //{
            //    var items = this.ParseEventListeners(sequenceFlow.ExtensionElements);
            //    foreach (var item in items)
            //        sequenceFlow.EventListeners.Add(item);
            //}

            return sequenceFlow;
        }
    }
}
