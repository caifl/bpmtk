using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine.Scripting
{
    [Serializable]
    public class ScriptingException : EngineException
    {
        public ScriptingException(string message) : base(message)
        {
        }

        public ScriptingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScriptingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
