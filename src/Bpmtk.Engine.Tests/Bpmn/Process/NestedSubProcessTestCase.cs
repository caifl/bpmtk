using Bpmtk.Engine.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class NestedSubProcessTestCase : BpmtkTestCase
    {
        public NestedSubProcessTestCase(ITestOutputHelper output) : base(output)
        {
        }

        [Fact] public async Task Execute()
        {
            await this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.SubProcess.SubProcessTest.testNestedSimpleSubProcess.bpmn20.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("nestedSimpleSubProcess");

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            // After process start, only task 0 should be active
            var tasks = await query.ListAsync();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Task in subprocess");

            // Completing Task in subprocess will finish the process.
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = await query.ListAsync();
            Assert.True(1 == tasks.Count); 
            Assert.True(tasks[0].Name == "Task after subprocesses");
            //this.Commit();

            await taskManager.CompleteAsync(tasks[0].Id);
            AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
