using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Bpmtk.Engine.Bpmn2
{
    [Serializable]
    public class BpmnError : EngineException
    {
        public BpmnError(string errorCode, string message) 
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        protected BpmnError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public virtual string ErrorCode
        {
            get;
        }
    }
}
