using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialUserTasksEmptyCollectionTestCase : BpmtkTestCase
    {
        public SequentialUserTasksEmptyCollectionTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialUserTasksEmptyCollection.bpmn20.xml");

            var map = new Dictionary<string, object>();
            //map.Add("sum", 0);
            map.Add("messages", new string[0]);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("miSequentialUserTasksEmptyCollection",
                map);

            var query = this.context.HistoryManager.CreateActivityQuery();
            var list = await query
                .SetProcessInstanceId(pi.Id)
                .SetIsMIRoot(false)
                .SetActivityType("UserTask")
                .ListAsync();

            Assert.True(list.Count == 0);

            //no tasks
            var tasks = await this.taskManager.CreateQuery()
                .SetProcessInstanceId(pi.Id)
                .ListAsync();

            Assert.True(tasks.Count == 0);

            var tokens = await this.runtimeManager.GetActiveTokensAsync(pi.Id);

            //
            Assert.True(tokens.Count == 0);

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
