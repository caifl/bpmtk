using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Bpmn2.Parser
{
    public interface IParseContext
    {
        BpmnFactory BpmnFactory
        {
            get;
        }

        void PushScope(FlowElementScope scope);

        FlowElementScope PopScope();

        FlowElementScope PeekScope();
        //FlowElementParseContext SubProcessContext
        //{
        //    get;
        //    set;
        //}
    }
}
