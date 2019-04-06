using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn
{
    public class NestedSubProcessTestCase : BpmtkTestCase
    {
        public NestedSubProcessTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override void Execute()
        {
            this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.SubProcess.SubProcessTest.testNestedSimpleSubProcess.bpmn20.xml");

            var pi = this.runtimeService.StartProcessInstanceByKey("nestedSimpleSubProcess");

            var query = this.taskService.CreateQuery()
                .SetState(Tasks.TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            // After process start, only task 0 should be active
            var tasks = query.List();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Task in subprocess");

            // Completing Task in subprocess will finish the process.
            taskService.Complete(tasks[0].Id);
            tasks = query.List();
            Assert.True(1 == tasks.Count); 
            Assert.True(tasks[0].Name == "Task after subprocesses");
            //this.unitOfWork.Commit();

            taskService.Complete(tasks[0].Id);
            AssertProcessInstanceEnd(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
