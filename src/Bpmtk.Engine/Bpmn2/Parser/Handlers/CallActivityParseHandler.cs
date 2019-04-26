using Bpmtk.Bpmn2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Bpmtk.Engine.Bpmn2.Parser.Handlers
{
    class CallActivityParseHandler : ActivityParseHandler
    {
        public override object Create(IFlowElementsContainer parent, IParseContext context, XElement element)
        {
            var callActivity = context.BpmnFactory.CreateCallActivity();
            parent.FlowElements.Add(callActivity);

            var calledElement = element.GetAttribute("calledElement");
            callActivity.CalledElement = calledElement;

            base.Init(callActivity, context, element);

            return callActivity;
        }
    }
}
