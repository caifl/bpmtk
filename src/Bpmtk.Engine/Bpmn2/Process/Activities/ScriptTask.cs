using System;
using Bpmtk.Engine.Runtime;
using Bpmtk.Engine.Scripting;

namespace Bpmtk.Engine.Bpmn2
{
    public class ScriptTask : Task
    {
        public virtual string Script
        {
            get;
            set;
        }

        public virtual string ScriptFormat
        {
            get;
            set;
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Execute(ExecutionContext executionContext)
        {
            if (!string.IsNullOrEmpty(this.Script))
            {
                var scriptingContext = new ScriptingContext(executionContext);
                var engine = new JavascriptEngine();

                var scope = engine.CreateScope(scriptingContext);
                engine.Execute(this.Script, scope);
                //var context = executionContext.Context;
                //var scriptEngineManager = context.GetService<IScriptEngineManager>();

                //var engine = scriptEngineManager.Get(this.ScriptFormat);
                //engine.Execute(this.Script);
            }

            base.Execute(executionContext);
        }

    }
}
