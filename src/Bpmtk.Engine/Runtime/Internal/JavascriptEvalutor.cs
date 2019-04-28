using Bpmtk.Engine.Scripting;
using Bpmtk.Engine.Utils;
using System;

namespace Bpmtk.Engine.Runtime.Internal
{
    public class JavascriptEvalutor : IEvaluator
    {
        private JavascriptEngine engine;
        private IScriptingScope scope;

        public JavascriptEvalutor(ExecutionContext executionContext)
        {
            engine = new JavascriptEngine();
            scope = engine.CreateScope(new ScriptingContext(executionContext));
        }

        public virtual object Evalute(string script)
        {
            script = StringHelper.ExtractExpression(script);
            return engine.Execute(script, scope);
        }

        public virtual TValue Evalute<TValue>(string script)
        {
            var result = this.Evalute(script);
            if (result != null)
                return (TValue)result;

            return default(TValue);
        }

        public virtual string EvaluteToString(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text,
                StringHelper.JuelSearchPattern,
            new System.Text.RegularExpressions.MatchEvaluator((m) =>
            {
                var expr = m.Value;
                object value = engine.Execute(expr, scope);
                if (value != null)
                    return value.ToString();

                return string.Empty;
            }));
        }
    }
}
