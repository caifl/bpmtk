using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine
{
    [Serializable]
    public class StateTransitionNotAllowedException : EngineException
    {
        public StateTransitionNotAllowedException(string message) : base(message)
        {
        }

        public StateTransitionNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StateTransitionNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
