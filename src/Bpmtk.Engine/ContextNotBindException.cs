using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine
{
    [Serializable]
    public class ContextNotBindException : EngineException
    {
        public ContextNotBindException(string message) : base(message)
        {
        }

        public ContextNotBindException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContextNotBindException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
