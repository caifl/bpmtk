using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine.Runtime
{
    [Serializable]
    public class RuntimeException : EngineException
    {
        public RuntimeException(string message) : base(message)
        {
        }

        public RuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
