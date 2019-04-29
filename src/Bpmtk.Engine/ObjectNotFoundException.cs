using System;

namespace Bpmtk.Engine
{
    [Serializable]
    public class ObjectNotFoundException : EngineException
    {
        public ObjectNotFoundException(string entityName) 
            : base($"The specified '{entityName}' does not exists.")
        {
        }

        public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
