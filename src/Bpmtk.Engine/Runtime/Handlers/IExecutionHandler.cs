using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime.Handlers
{
    interface IExecutionHandler
    {
        void Execute(ExecutionContext executionContext);
    }
}
