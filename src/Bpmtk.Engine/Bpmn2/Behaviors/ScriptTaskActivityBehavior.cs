using System;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class ScriptTaskActivityBehavior : TaskActivityBehavior
    {
        public override async System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            var scriptTask = executionContext.Node as ScriptTask;
            if (scriptTask == null)
                throw new InvalidOperationException();

            var script = scriptTask.Script;
            if (!string.IsNullOrEmpty(script))
            {
                var evaluator = executionContext.GetEvalutor(scriptTask.ScriptFormat);
                evaluator.Evalute(script);
                return;
            }

            await base.LeaveAsync(executionContext);
        }
    }
}
