using System.Collections.Generic;
using Bpmtk.Engine.Runtime;

namespace Bpmtk.Engine.Bpmn2.Behaviors
{
    /// <summary>
    /// Upon activation, the data in the associated Message is assigned from the data in the Data Input of
    /// the Send Task.The Message is sent and the Send Task completes.
    /// </summary>
    public class SendTaskActivityBehavior : TaskActivityBehavior
    {
        //public override System.Threading.Tasks.Task ExecuteAsync(ExecutionContext executionContext)
        //{
        //    var task = executionContext.Node as SendTask;

        //    //
        //    var messageData = new Dictionary<string, object>();
        //    if (task.IOSpecification != null)
        //    {
        //        var dataInputs = task.IOSpecification.DataInputs;
        //        foreach (var dataInput in dataInputs)
        //        {
        //            var value = executionContext.GetVariableLocal(dataInput.Id);
        //            messageData.Add(dataInput.Id, value);
        //        }
        //    }

        //    //sendMessage(name, messageData);
        //}
    }
}
