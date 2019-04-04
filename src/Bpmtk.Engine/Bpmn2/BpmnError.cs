using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Bpmn2
{
    public class BpmnError : Exception
    {
        public BpmnError(string message) : base(message)
        {
        }
    }
}
