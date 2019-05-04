using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    /// <summary>
    /// If the Receive Task’s instantiate attribute is set to true, 
    /// the Receive Task itself can start a new Process instance.
    /// </summary>
    public class ReceiveTaskActivityBehavior : TaskActivityBehavior, ISignallableActivityBehavior
    {
        /// <summary>
        /// Upon activation, the Receive Task begins waiting for the associated Message. When the
        /// Message arrives, the data in the Data Output of the Receive Task is assigned from the data in the Message,
        /// and Receive Task completes.
        /// </summary>
        public System.Threading.Tasks.Task SignalAsync(ExecutionContext executionContext, 
            string signalEvent, 
            IDictionary<string, object> signalData)
        {
            var task = executionContext.Node as ReceiveTask;

            if (signalData != null
                && signalData.Count > 0
                && task.IOSpecification != null
                && task.IOSpecification.DataOutputs.Count > 0)
            {
                foreach (var dataOutput in task.IOSpecification.DataOutputs)
                {
                    object value = null;
                    if (signalData.TryGetValue(dataOutput.Id, out value))
                    {
                        executionContext.SetVariableLocal(dataOutput.Id, value);
                    }
                }
            }

            return base.LeaveAsync(executionContext);
        }

        public override System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        {
            //Waiting for signal.

            //return base.ExecuteAsync(executionContext);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
