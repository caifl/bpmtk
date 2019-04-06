using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine
{
    [Serializable]
    public class EngineException : Exception
    {
        public EngineException(string message) : base(message)
        {
        }

        public EngineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EngineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
