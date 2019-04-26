using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class DataObjectScopeTestCase : BpmtkTestCase
    {
        public DataObjectScopeTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override async Task Execute()
        {
            await this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.SubProcess.SubProcessTest.testDataObjectScope.bpmn20.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("dataObjectScope");
            //var value = pi.GetVariable("dObj123");
            //value = pi.GetVariable("noData123");

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            // After process start, only task 0 should be active
            var tasks = query.List();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Complete Task A");

            var value = tasks[0].GetVariable("noData123");

            // Completing Task in subprocess will finish the process.
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = query.List();
            Assert.True(1 == tasks.Count);

            var var1 = tasks[0].GetVariable("dObj456");
            Assert.True("Testing456".Equals(var1));
            Assert.True(tasks[0].Name == "Complete SubTask");

            await taskManager.CompleteAsync(tasks[0].Id);
            //Assert.True("Testing456".Equals(var1));
            //taskService.Complete(tasks[0].Id);
            tasks = query.List();
            await taskManager.CompleteAsync(tasks[0].Id);
            AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
