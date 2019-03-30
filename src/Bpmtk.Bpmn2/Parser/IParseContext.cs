using System;

namespace Bpmtk.Bpmn2.Parser
{
    public interface IParseContext
    {
        BpmnFactory BpmnFactory
        {
            get;
        }

        void AddReferenceRequest<TBaseElement>(string id, Action<TBaseElement> action)
            where TBaseElement : IBaseElement;

        void Push(FlowElement flowElement);

        void Push(BaseElement baseElement);
    }
}
