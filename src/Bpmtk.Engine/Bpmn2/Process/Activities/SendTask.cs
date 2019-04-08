using System;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    /// <summary>
    /// Upon activation, the data in the associated Message is assigned from the data in the Data Input of
    /// the Send Task.The Message is sent and the Send Task completes.
    /// </summary>
    public class SendTask : Task
    {
        public virtual string Implementation
        {
            get;
            set;
        }

        public virtual Message MessageRef
        {
            get;
            set;
        }

        public virtual Operation OperationRef
        {
            get;
            set;
        }

        public override void Execute(ExecutionContext executionContext)
        {
            //
            var messageData = new Dictionary<string, object>();
            if(this.IOSpecification != null)
            {
                var dataInputs = this.IOSpecification.DataInputs;
                foreach(var dataInput in dataInputs)
                {
                    var value = executionContext.GetVariableLocal(dataInput.Id);
                    messageData.Add(dataInput.Id, value);
                }
            }

            //sendMessage(name, messageData);
        }

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
