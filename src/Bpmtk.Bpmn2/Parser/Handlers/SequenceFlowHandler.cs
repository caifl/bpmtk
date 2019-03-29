using System;
using System.Xml.Linq;

namespace Bpmtk.Bpmn2.Parser
{
    class SequenceFlowHandler<TFlowElementContainer> : FlowElementHandler<TFlowElementContainer, SequenceFlow>
        where TFlowElementContainer : IFlowElementsContainer
    {
        public SequenceFlowHandler()
        {
            this.handlers.Add("conditionExpression", new ExpressionHandler<SequenceFlow>((p, c, e, x) =>
            {
                p.ConditionExpression = x;
            }));
        }

        public override SequenceFlow Create(TFlowElementContainer parent, IParseContext context, XElement element)
        {
            var sequenceFlow = base.Create(parent, context, element);

            var scope = context.PeekScope();

            var sourceRef = element.GetAttribute("sourceRef");
            var targetRef = element.GetAttribute("targetRef");

            scope.AddSourceRef(sourceRef, sequenceFlow);
            scope.AddTargetRef(targetRef, sequenceFlow);

            if (sequenceFlow.ExtensionElements != null)
            {
                var items = this.ParseEventListeners(sequenceFlow.ExtensionElements);
                foreach (var item in items)
                    sequenceFlow.EventListeners.Add(item);
            }

            return sequenceFlow;
        }

        protected override SequenceFlow New(IParseContext context, XElement element)
            => context.BpmnFactory.CreateSequenceFlow();
    }
}
