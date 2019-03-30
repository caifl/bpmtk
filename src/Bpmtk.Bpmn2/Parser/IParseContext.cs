using System;

namespace Bpmtk.Bpmn2.Parser
{
    public interface IParseContext
    {
        BpmnFactory BpmnFactory
        {
            get;
        }

        void AddReferenceRequest<TObject>(string id, Action<TObject> callback);

        //void AddReferenceRequest(ObjectReferenceRequest value);

        void Push(ItemDefinition itemDefinition);

        void Push(Message message);

        void Push(FlowElement flowElement);

        //void PushScope(FlowElementScope scope);

        //FlowElementScope PopScope();

        //FlowElementScope PeekScope();
        //FlowElementParseContext SubProcessContext
        //{
        //    get;
        //    set;
        //}
    }
}
