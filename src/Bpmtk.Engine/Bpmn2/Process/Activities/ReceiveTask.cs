using System;
using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2
{
    public class ReceiveTask : Task
    {
        public virtual string Implementation
        {
            get;
            set;
        }

        public virtual bool Instantiate
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

        public override void Accept(IFlowNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// If the Receive Task’s instantiate attribute is set to true, 
        /// the Receive Task itself can start a new Process instance.
        /// </summary>
        public override void Execute(ExecutionContext executionContext)
        {
            //Waiting ...

            //base.Execute(executionContext);
        }

        /// <summary>
        /// Upon activation, the Receive Task begins waiting for the associated Message. When the
        /// Message arrives, the data in the Data Output of the Receive Task is assigned from the data in the Message,
        /// and Receive Task completes.
        /// </summary>
        public override void Signal(ExecutionContext executionContext, string signalName, IDictionary<string, object> signalData)
        {
            if(signalData != null 
                && signalData.Count > 0
                && this.IOSpecification != null
                && this.IOSpecification.DataOutputs.Count > 0)
            {
                foreach(var dataOutput in this.IOSpecification.DataOutputs)
                {
                    object value = null;
                    if(signalData.TryGetValue(dataOutput.Id, out value))
                    {
                        executionContext.SetVariableLocal(dataOutput.Id, value);
                    }
                }
            }

            //evaluate DataOutputAssociations.
            //...

            base.Leave(executionContext);
        }
    }
}
