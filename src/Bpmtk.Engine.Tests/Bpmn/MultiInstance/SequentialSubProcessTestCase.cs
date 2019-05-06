using Bpmtk.Engine.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialSubProcessTestCase : BpmtkTestCase
    {
        public SequentialSubProcessTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubProcess.bpmn20.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miSequentialSubprocess");

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            for (int i = 0; i < 4; i++)
            {
                var tasks = await query.ListAsync();
                tasks = tasks.OrderBy(x => x.Name).ToList();
                Assert.True(2 == tasks.Count);

                Assert.True("task one" == tasks[0].Name);
                Assert.True("task two" == tasks[1].Name);

                await this.taskManager.CompleteAsync(tasks[0].Id);
                await this.taskManager.CompleteAsync(tasks[1].Id);

                if (i != 3)
                {
                    //var activeTokens = await this.runtimeManager.GetActiveTokensAsync(pi.Id);
                    var activities = await this.runtimeManager.GetActiveActivityIdsAsync(pi.Id);
                    Assert.True(3 == activities.Count);
                }
            }

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
