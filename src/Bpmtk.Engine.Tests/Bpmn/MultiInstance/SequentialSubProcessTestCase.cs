using Bpmtk.Engine.Models;
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
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubProcessEndEvent.bpmn20.xml");

            var map = new Dictionary<string, object>();
            //map.Add("sum", 0);
            //map.Add("nrOfLoops", 5);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miSequentialSubprocess",
                map);

            //var myVar = pi.GetVariable("sum");
            //Assert.True(10 == Convert.ToInt32(myVar));

            //var actInsts = this.runtimeService.CreateActivityQuery()
            //    .List();

            //var list = actInsts.Where(x => x.ActivityType == "SubProcess").ToList();
            //Assert.True(list.Count == 1);

            

            // After process start, only task one should be active
            //var tasks = query.List();
            //Assert.True(tasks.Count == 1);
            //Assert.True(tasks[0].Name == "task one");

            //while (tasks.Count > 0)
            //{
            //    this.taskService.Complete(tasks[0].Id);
            //    tasks = query.List();
            //}

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);
            for (int i = 0; i < 4; i++)
            {
                var tasks = query.List();
                Assert.True(1 == tasks.Count);

                Assert.True("task one" == tasks[0].Name);

                await taskManager.CompleteAsync(tasks[0].Id);

                // Last run, the execution no longer exists
                if (i != 3)
                {
                    var activities = await this.runtimeManager.GetActiveActivityIdsAsync(pi.Id);
                    Assert.NotNull(activities);
                    Assert.True(2 == activities.Count());
                }
            }

            //foreach(var item in list)
            //{
            //    Assert.True(item.StartTime != null);
            //    Assert.True(item.EndTime != null);
            //    Assert.True(item.State == Runtime.ExecutionState.Completed);
            //}

            //var tokenQuery = this.runtimeService.CreateTokenQuery();
            //tokenQuery.SetProcessInstance(pi.Id);

            //var tokens = tokenQuery.List();

            //Assert.True(tokens.Count == 1);

            ////trigger
            //this.runtimeService.Trigger(tokens[0].Id);

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
