using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class SingleMessageStartEventTestCase : BpmtkTestCase
    {
        public SingleMessageStartEventTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Event.MessageStartEventTest.testSingleMessageStartEvent.bpmn20.xml");

            var pi = this.runtimeService.StartProcessInstanceByMessage("newInvoiceMessage", new
            {
                a = "a"
            });

            var tasks = this.taskService.CreateQuery().SetState(Tasks.TaskState.Active).List();
            Assert.True(tasks.Count == 1);

            this.taskService.Complete(tasks[0].Id);

            this.AssertProcessInstanceEnd(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
