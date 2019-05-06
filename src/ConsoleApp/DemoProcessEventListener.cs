using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Runtime;

namespace ConsoleApp
{
    class DemoProcessEventListener : IProcessEventListener
    {
        public void ActivityStart(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity started: {0}[{1}]", node.Id, node.Name);

            
        }

        public void ActivityEnd(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity ended: {0}[{1}]", node.Id, node.Name);

            
        }

        public void ProcessEnd(IExecutionContext executionContext)
        {
            var processInstance = executionContext.ProcessInstance;
            //var node = executionContext.Node;

            Console.WriteLine("Process ended: {0}[{1}]", processInstance.Id, processInstance.Name);

            
        }

        public void ActivityReady(IExecutionContext executionContext)
        {
            var node = executionContext.Node;

            Console.WriteLine("Activity '{0}[{1}]' token entered.", node.Id, node.Name);

            
        }

        public void ProcessStart(IExecutionContext executionContext)
        {
            var processInstance = executionContext.ProcessInstance;

            Console.WriteLine("Process started: '{0}[{1}]'.", processInstance.Id, processInstance.Name);

            
        }

        public void TakeTransition(IExecutionContext executionContext)
        {
            var transition = executionContext.Transition;

            Console.WriteLine("Transition '{0}[{1}]' has been taken.", transition.Id, transition.Name);

            
        }
    }
}
