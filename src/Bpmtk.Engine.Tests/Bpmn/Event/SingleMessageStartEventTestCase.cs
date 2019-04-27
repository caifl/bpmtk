using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class SingleMessageStartEventTestCase : BpmtkTestCase
    {
        public SingleMessageStartEventTestCase(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Event.MessageStartEventTest.testSingleMessageStartEvent.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("a", "a");

            var pi = this.runtimeManager.StartProcessByMessageAsync("newInvoiceMessage", 
                map);

            var tasks = this.taskManager.CreateQuery().SetState(TaskState.Active).List();
            Assert.True(tasks.Count == 1);

            await taskManager.CompleteAsync(tasks[0].Id);

            this.AssertProcessEnded(pi.Id);
        }
    }
}
