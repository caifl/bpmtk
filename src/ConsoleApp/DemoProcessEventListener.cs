using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Runtime;

namespace ConsoleApp
{
    class DemoProcessEventListener : IProcessEventListener
    {
        public Task ActivatedAsync(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity started: {0}[{1}]", node.Id, node.Name);

            return Task.CompletedTask;
        }

        public Task EndedAsync(IExecutionContext executionContext)
        {
            var processInstance = executionContext.ProcessInstance;
            //var node = executionContext.Node;

            Console.WriteLine("Process ended: {0}[{1}]", processInstance.Id, processInstance.Name);

            return Task.CompletedTask;
        }

        public Task EnterNodeAsync(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity '{0}[{1}]' token entered.", node.Id, node.Name);

            return Task.CompletedTask;
        }

        public Task LeaveNodeAsync(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity ended: '{0}[{1}]'.", node.Id, node.Name);

            return Task.CompletedTask;
        }

        public Task StartedAsync(IExecutionContext executionContext)
        {
            var processInstance = executionContext.ProcessInstance;

            Console.WriteLine("Process started: '{0}[{1}]'.", processInstance.Id, processInstance.Name);

            return Task.CompletedTask;
        }

        public Task TakeTransitionAsync(IExecutionContext executionContext)
        {
            var transition = executionContext.Transition;

            Console.WriteLine("Transition '{0}[{1}]' has been taken.", transition.Id, transition.Name);

            return Task.CompletedTask;
        }
    }
}
