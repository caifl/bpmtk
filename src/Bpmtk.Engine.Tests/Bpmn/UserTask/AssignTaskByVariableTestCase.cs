using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.UserTask
{
    public class AssignTaskByVariableTestCase : BpmtkTestCase
    {
        public AssignTaskByVariableTestCase(ITestOutputHelper output) : base(output)
        {
        }

      
        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Bpmn.UserTask.AssignTaskByVariableTestCase.bpmn.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("AssignTaskByVariableTestCase");

            var query = this.taskManager.CreateQuery()
                .SetProcessInstanceId(pi.Id);

            var tasks = await query.ListAsync();
            Assert.True(tasks.Count == 1);

            Assert.True(tasks[0].Assignee == this.context.UserId);

            await this.taskManager.CompleteAsync(tasks[0].Id);

            this.AssertProcessEnded(pi.Id);

            //this.Commit();
        }
    }
}
