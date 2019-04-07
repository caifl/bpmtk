using Bpmtk.Engine.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Runtime
{
    class ScriptingContext : IVariableResolver
    {
        private readonly ExecutionContext executionContext;

        public ScriptingContext(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public virtual bool Resolve(string name, out object value)
        {
            if (name == "execution")
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
