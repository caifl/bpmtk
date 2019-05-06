using System;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    public class ScriptTaskActivityBehavior : TaskActivityBehavior
    {
        public override void Execute(ExecutionContext executionContext)
        {
            var scriptTask = executionContext.Node as ScriptTask;
            if (scriptTask == null)
                throw new InvalidOperationException();

            var script = scriptTask.Script;
            if (!string.IsNullOrEmpty(script))
            {
                var evaluator = executionContext.GetEvaluator(scriptTask.ScriptFormat);
                evaluator.Evaluate(script);
            }

            base.Leave(executionContext);
        }
    }
}
