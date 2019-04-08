using System;
using System.Runtime.Serialization;

namespace Bpmtk.Engine.Repository
{
    [Serializable]
    public class DeploymentException : EngineException
    {
        public DeploymentException(string message) : base(message)
        {
        }

        public DeploymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeploymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
