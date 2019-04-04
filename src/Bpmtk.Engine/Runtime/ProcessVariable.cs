using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Variables;

namespace Bpmtk.Engine.Runtime
{
    public class ProcessVariable : VariableInstance
    {
        protected ProcessInstance processInstance;

        protected ProcessVariable()
        { }

        public ProcessVariable(ProcessInstance processInstance,
            string name,
            IVariableType type,
            object value
            ) : base(name, value)
        {
            if (processInstance == null)
                throw new ArgumentNullException(nameof(processInstance));

            this.processInstance = processInstance;
            this.type = type;
        }

        public ProcessVariable(ProcessInstance processInstance,
            string name,
            object value
            ) : base(name, value)
        {
            if (processInstance == null)
                throw new ArgumentNullException(nameof(processInstance));

            this.processInstance = processInstance;
        }

        public virtual ProcessInstance ProcessInstance
        {
            get => this.processInstance;
        }
    }
}
