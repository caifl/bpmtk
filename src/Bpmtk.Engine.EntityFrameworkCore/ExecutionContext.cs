using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine
{
    class ExecutionContext
    {
        private readonly Token token;
        private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public ExecutionContext(Token token)
        {
            this.token = token;
        }

        public object GetVariable(string name)
        {
            Variable variable = null;
            if(variables.TryGetValue(name, out variable))
            {
                return null;
            }

            var current = this.token;

            do
            {
                variable = current.GetVariable(name);
                if (variable != null)
                {
                    this.variables.Add(name, variable);
                    return null;
                }

                current = current.Parent;
            }
            while (current.Parent != null);

            variable = this.token.ProcessInstance.GetVariable(name);

            return null;
        }
    }
}
