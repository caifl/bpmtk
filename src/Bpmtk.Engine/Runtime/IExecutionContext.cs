using System;
using System.Collections.Generic;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public interface IExecutionContext
    {
        ProcessInstance ProcessInstance
        {
            get;
        }

        /// <summary>
        /// Get the token object.
        /// </summary>
        Token Token
        {
            get;
        }

        /// <summary>
        /// Gets current node.
        /// </summary>
        FlowNode Node
        {
            get;
        }

        /// <summary>
        /// Gets current transition.
        /// </summary>
        SequenceFlow Transition
        {
            get;
        }

        /// <summary>
        /// Gets current context.
        /// </summary>
        IContext Context
        {
            get;
        }

        object GetVariable(string name);

        void SetVariable(string name, object value);

        object GetVariableLocal(string name);

        void SetVariableLocal(string name, object value);
    }
}
