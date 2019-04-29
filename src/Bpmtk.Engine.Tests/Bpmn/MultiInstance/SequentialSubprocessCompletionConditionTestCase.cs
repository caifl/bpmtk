using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialSubprocessCompletionConditionTestCase : BpmtkTestCase
    {
        public SequentialSubprocessCompletionConditionTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubProcessCompletionCondition.bpmn20.xml");

            var map = new Dictionary<string, object>();
            //map.Add("collection", new string[0] { });
            //map.Add("nrOfLoops", 5);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miSequentialSubprocessCompletionCondition",
                map);

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            //var tasks = query.List();
            //Assert.True(tasks.Count == 0);

            //while(tasks.Count > 0)
            //{
            //    //Assert.True(1 == tasks.Count);

            //    //Assert.True("task one" == tasks[0].Name);

            //    taskService.Complete(tasks[0].Id);
            //    tasks = query.List();
            //}

            for (int i = 0; i < 3; i++)
            {
                var tasks = await query.ListAsync();
                tasks = tasks.OrderBy(x => x.Name).ToList();

                Assert.True(2 == tasks.Count);

                Assert.True("task one" == tasks[0].Name);
                Assert.True("task two" == tasks[1].Name);

                await taskManager.CompleteAsync(tasks[0].Id);
                await taskManager.CompleteAsync(tasks[1].Id);
            }

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
