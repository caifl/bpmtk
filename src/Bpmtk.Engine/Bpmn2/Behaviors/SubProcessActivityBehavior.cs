using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class SubProcessActivityBehavior : ActivityBehavior
    {
        public override Task ExecuteAsync(ExecutionContext executionContext)
        {
            var subProcess = executionContext.Node as Bpmtk.Bpmn2.SubProcess;
            //var initialNode = subProcess.FlowElements.OfType<StartEvent>().FirstOrDefault();

            //var subProcessExecution = executionContext.CreateSubProcessContext();
            //subProcessExecution.Start(initialNode);

            var context = executionContext.Context;

            var startEvent = subProcess.FlowElements
                .OfType<Bpmtk.Bpmn2.StartEvent>()
                .Where(x => x.EventDefinitions.Count == 0 && x.EventDefinitionRefs.Count == 0)
                .FirstOrDefault();

            if (startEvent == null)
                throw new RuntimeException($"No initial activity found for subProcess '{subProcess.Id}'.");

            var token = executionContext.Token;
            var scope = executionContext.ActivityInstance;

            //create sub-process scope token.
            var child = token.CreateToken();
            child.Node = startEvent;
            //child.Scope = scope;

            return Task.CompletedTask;

            //var runtimeManager = executionContext.Context.RuntimeManager;
            //runtimeManager.SaveAsync()
            //var subExecution = ExecutionContext.Create(context, child);
            //startEvent.Enter(subExecution);
        }
    }
}
