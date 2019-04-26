using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.Gateway
{
    public class NestedForkJoinTestCase : BpmtkTestCase
    {
        public NestedForkJoinTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override async Task Execute()
        {
            await this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Gateway.ParallelGatewayTest.testNestedForkJoin.bpmn20.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("nestedForkJoin");

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            // After process start, only task 0 should be active
            var tasks = query.List();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Task 0");

            // Completing task 0 will create Task A and B
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = query.List();
            Assert.True(2 == tasks.Count);
            Assert.True("Task A" == tasks[0].Name);
            Assert.True("Task B" == tasks[1].Name);         

            // Completing task A should not trigger any new tasks
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = query.List();
            Assert.True(1 == tasks.Count);
            Assert.True("Task B" == tasks[0].Name);

            // Completing task B creates tasks B1 and B2
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = query.List();
            Assert.True(2 == tasks.Count);
            Assert.True("Task B1" == tasks[0].Name);
            Assert.True("Task B2" == tasks[1].Name);
            //this.unitOfWork.Commit();


            // Completing B1 and B2 will activate both joins, and process reaches
            // task C
            await taskManager.CompleteAsync(tasks[0].Id);
            await taskManager.CompleteAsync(tasks[1].Id);
            tasks = query.List();
            Assert.True(1 == tasks.Count);
            Assert.True("Task C" == tasks[0].Name);

            // Completing Task C will finish the process.
            await taskManager.CompleteAsync(tasks[0].Id);
            tasks = query.List();
            Assert.True(0 == tasks.Count); //all tasks completed.
            AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
