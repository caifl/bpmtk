using Bpmtk.Engine.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class ParallelScriptTasksTestCase : BpmtkTestCase
    {
        public ParallelScriptTasksTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testParallelScriptTasks.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("sum", 0);
            map.Add("nrOfLoops", 5);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miParallelScriptTask",
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

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
