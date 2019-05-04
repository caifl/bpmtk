using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialScriptTasksTestCase : BpmtkTestCase
    {
        public SequentialScriptTasksTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialScriptTasks.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("sum", 0);
            map.Add("nrOfLoops", 5);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miSequentialScriptTask",
                map);

            var myVar = pi.GetVariable("sum");
            Assert.True(10 == Convert.ToInt32(myVar));

            var query = this.context.HistoryManager.CreateActivityQuery();
            var list = await query
                .SetProcessInstanceId(pi.Id)
                .SetIsMIRoot(false)
                .SetActivityType("ScriptTask")
                .ListAsync();

            Assert.True(list.Count == 5);
            foreach (var item in list)
            {
                Assert.True(item.StartTime != null);
                Assert.True(item.State == ExecutionState.Completed);
            }

            var tokens = await this.runtimeManager.GetActiveTokensAsync(pi.Id);

            //
            Assert.True(tokens.Count == 1);

            //trigger
            await this.runtimeManager.TriggerAsync(tokens[0].Id);

            //var query = this.taskService.CreateQuery().SetState(TaskState.Active);

            //var tasks = query.List();
            //while(tasks.Count > 0)
            //{
            //    this.taskService.Complete(tasks[0].Id);
            //    tasks = query.List();
            //}
            //Assert.True(tasks.Count == 1);

            ////get variable from task-instance.
            //myVar = tasks[0].GetVariable("myVar");
            //Assert.True("test123".Equals(myVar));

            //this.taskService.Complete(tasks[0].Id);

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
