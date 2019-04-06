using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class DataObjectScopeTestCase : BpmtkTestCase
    {
        public DataObjectScopeTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override void Execute()
        {
            this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.SubProcess.SubProcessTest.testDataObjectScope.bpmn20.xml");

            var pi = this.runtimeService.StartProcessInstanceByKey("dataObjectScope");
            //var value = pi.GetVariable("dObj123");
            //value = pi.GetVariable("noData123");

            var query = this.taskService.CreateQuery()
                .SetState(Tasks.TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            // After process start, only task 0 should be active
            var tasks = query.List();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Complete Task A");

            var value = tasks[0].GetVariable("noData123");

            // Completing Task in subprocess will finish the process.
            taskService.Complete(tasks[0].Id);
            tasks = query.List();
            Assert.True(1 == tasks.Count);

            var var1 = tasks[0].GetVariable("dObj456");
            Assert.True("Testing456".Equals(var1));
            Assert.True(tasks[0].Name == "Complete SubTask");

            taskService.Complete(tasks[0].Id);
            //Assert.True("Testing456".Equals(var1));
            //taskService.Complete(tasks[0].Id);
            tasks = query.List();
            taskService.Complete(tasks[0].Id);
            AssertProcessInstanceEnd(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
