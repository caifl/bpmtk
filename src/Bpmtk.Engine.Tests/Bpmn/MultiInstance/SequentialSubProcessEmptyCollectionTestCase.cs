using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialSubProcessEmptyCollectionTestCase : BpmtkTestCase
    {
        public SequentialSubProcessEmptyCollectionTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubprocessEmptyCollection.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("collection", new string[0] { });
            //map.Add("nrOfLoops", 5);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("testSequentialSubProcessEmptyCollection",
                map);

            var query = this.taskManager.CreateQuery()
                .SetState(TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            var tasks = query.List();
            Assert.True(tasks.Count == 0);

            while(tasks.Count > 0)
            {
                //Assert.True(1 == tasks.Count);

                //Assert.True("task one" == tasks[0].Name);

                await taskManager.CompleteAsync(tasks[0].Id);
                tasks = query.List();
            }

            this.AssertProcessEnded(pi.Id);

            this.Commit();
        }
    }
}
