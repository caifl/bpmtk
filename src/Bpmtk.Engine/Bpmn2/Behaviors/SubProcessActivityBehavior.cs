using System;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    class SubProcessActivityBehavior : ActivityBehavior
    {
        public override async Task ExecuteAsync(ExecutionContext executionContext)
        {
            var subProcess = executionContext.Node as Bpmtk.Bpmn2.SubProcess;

            var startEvent = subProcess.FlowElements
                .OfType<Bpmtk.Bpmn2.StartEvent>()
                .Where(x => x.EventDefinitions.Count == 0 && x.EventDefinitionRefs.Count == 0)
                .FirstOrDefault();

            if (startEvent == null)
                throw new RuntimeException($"No initial activity found for subProcess '{subProcess.Id}'.");

            await executionContext.StartSubProcessAsync(startEvent, null);
        }
    }
}
