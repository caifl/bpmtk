using System;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Scripting;

namespace Bpmtk.Engine.Runtime.Internal
{
    public class ScriptingContext : IVariableResolver
    {
        private readonly ExecutionContext executionContext;

        public ScriptingContext(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public virtual bool Resolve(string name, out object value)
        {
            if (name == "context" 
                || name == "execution")
            {
                value = this;
                return true;
            }

            value = executionContext.GetVariable(name);

            return true;
        }

        public virtual ProcessInstance ProcessInstance => executionContext.ProcessInstance;

        public virtual object GetVariable(string name) => executionContext.GetVariable(name);

        public virtual void SetVariable(string name, object value) => executionContext.SetVariable(name, value);

    }
}
