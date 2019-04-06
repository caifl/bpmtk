using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Bpmtk.Engine.Scripting
{
    public class JavascriptEngine : IScriptEngine
    {
        public string Name => "jint";
        protected readonly ConcurrentDictionary<string, object> values = new ConcurrentDictionary<string, object>();
        protected readonly ConcurrentDictionary<string, Type> clrTypes = new ConcurrentDictionary<string, Type>();
        protected readonly List<Type> types = new List<Type>();
        public IEnumerable<string> Languages => new string[] { "text/javascript","javascript" };

        public virtual IScriptingScope CreateScope(IVariableResolver variableResolver)
        {
            var scope = new JavascriptScope(variableResolver);

            var enumerator = this.values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;
                scope.SetValue(entry.Key, entry.Value);
            }

            var enumerator2 = this.clrTypes.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                var entry = enumerator2.Current;
                scope.AddClrType(entry.Key, entry.Value);
            }

            return scope;
        }

        public virtual object Execute(string script, IScriptingScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var jsScope = scope as JavascriptScope;
            if (jsScope == null)
                throw new ArgumentException("Invalid argument 'scope'.");

            if (string.IsNullOrEmpty(script))
                return null;

            try
            {
                return jsScope.Engine.Execute(script)
                    .GetCompletionValue()?
                    .ToObject();
            }
            catch(Exception ex)
            {
                throw new ScriptingException($"The javascript execution errors; {ex.Message}.", ex);
            }
        }

        public virtual JavascriptEngine AddClrType(string name, Type clrType)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (clrType == null)
                throw new ArgumentNullException(nameof(clrType));

            this.clrTypes.AddOrUpdate(name, clrType, (k, v) => v);

            return this;
        }

        public virtual IReadOnlyDictionary<string, object> GetValues()
        {
            return new ReadOnlyDictionary<string, object>(this.values);
        }

        public virtual JavascriptEngine SetValue(string key, object value)
        {
            this.values.AddOrUpdate(key, value, (k, v) => value);

            return this;
        }

        public virtual JavascriptEngine SetValue(string key, Delegate value)
        {
            this.values.AddOrUpdate(key, value, (k, v) => value);

            return this;
        }

        public virtual IReadOnlyDictionary<string, Type> GetClrTypes()
        {
            return new ReadOnlyDictionary<string, Type>(this.clrTypes);
        }

        public virtual object CreateCompileUnit(string script)
        {
            var parser = new Esprima.JavaScriptParser(script);
            var program = parser.ParseProgram();

            return program;
        }

        public virtual object CreateCompileUnit(string script, 
            out bool isExpression)
        {
            try
            {
                isExpression = false;

                var parser = new Esprima.JavaScriptParser(script);
                var program = parser.ParseProgram();

                isExpression = this.IsExpression(program);

                return program;
            }
            catch(Exception ex)
            {
                throw new ScriptingException($"Javascript parse error occurs: {ex.Message}.", ex);
            }
        }

        protected virtual bool IsExpression(Esprima.Ast.Program compileUnit)
        {
            //if (compileUnit == null)
            //    throw new ArgumentNullException(nameof(compileUnit));

            //if (!(compileUnit is Esprima.Ast.Program))
            //    throw new ArgumentException("Invalid compileUnit.");

            var program = compileUnit as Esprima.Ast.Program;
            var body = program.Body;
            var statementCount = body.Count;
            if (statementCount != 1)
                return false;

            return body[0].Type == Esprima.Ast.Nodes.ExpressionStatement;
        }

        public object ExecuteCompileUnit(object compileUnit, IScriptingScope scope)
        {
            if (compileUnit == null)
                throw new ArgumentNullException(nameof(compileUnit));

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (!(compileUnit is Esprima.Ast.Program))
                throw new ArgumentException("Invalid compileUnit.");

            var jsScope = scope as JavascriptScope;
            if (jsScope == null)
                throw new ArgumentException("Invalid argument 'scope'.");

            try
            {
                return jsScope.Engine.Execute(compileUnit as Esprima.Ast.Program)
                    .GetCompletionValue()?
                    .ToObject();
            }
            catch (Exception ex)
            {
                throw new ScriptingException($"The javascript execution errors; {ex.Message}.", ex);
            }
        }
    }
}
