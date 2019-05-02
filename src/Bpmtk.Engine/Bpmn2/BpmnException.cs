using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine.Bpmn2
{
    [Serializable]
    public class BpmnException : EngineException
    {
        public BpmnException(string message) : base(message)
        {
        }

        public BpmnException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BpmnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
