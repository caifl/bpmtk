using Bpmtk.Engine.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Bpmn2
{
    interface IBpmnActivityBehavior
    {
        void Execute(ExecutionContext executionContext);

        void Leave(ExecutionContext executionContext, BpmnTransition transition = null);
    }
}
