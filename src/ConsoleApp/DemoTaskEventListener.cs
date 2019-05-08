using System;
using Bpmtk.Engine.Events;
using Bpmtk.Engine.Tasks;

namespace ConsoleApp
{
    class DemoTaskEventListener : TaskEventListener
    {
        public DemoTaskEventListener()
        {
        }

        public override void Completed(ITaskEvent taskEvent)
        {
            var task = taskEvent.Task;
            //task.GetVariable("");

            base.Completed(taskEvent);
        }
    }
}
