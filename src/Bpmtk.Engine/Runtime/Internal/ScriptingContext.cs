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
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name == "context" 
                || name == "execution")
            {
                value = this;
                return true;
            }

            //Resolve in process-instance scope.
            if (executionContext.TryGetVariable(name, out value))
                return true;

            //Resolve in process-engine scope.
            return executionContext.Engine.TryGetValue(name, out value);
        }

        public virtual ProcessInstance ProcessInstance => executionContext.ProcessInstance;

        public virtual object GetVariable(string name) => executionContext.GetVariable(name);

        public virtual object GetVariableLocal(string name) => executionContext.GetVariableLocal(name);

        public virtual void SetVariable(string name, object value) => executionContext.SetVariable(name, value);

        public virtual void SetVariableLocal(string name, object value) => executionContext.SetVariableLocal(name, value);
    }
}
