using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.Gateway
{
    public class ExclusiveGatewayTestCase : BpmtkTestCase
    {
        public ExclusiveGatewayTestCase(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Gateway.ExclusiveGatewayTest.testDefaultSequenceFlow.bpmn20.xml");

            //1. Set input = 2, to take default outgoing.
            var map = new Dictionary<string, object>();
            map.Add("input", 2);

            var pi = await this.runtimeManager.StartProcessByKeyAsync("exclusiveGwDefaultSequenceFlow",
                map);

            var query = this.taskManager
                .CreateQuery()
                .SetProcessInstanceId(pi.Id)
                .SetState(TaskState.Active);

            var tasks = await query.ListAsync();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Default input");
            await taskManager.CompleteAsync(tasks[0].Id);
            this.AssertProcessEnded(pi.Id);

            //2. Set input = 1, to take 'flow2'
            map = new Dictionary<string, object>();
            map.Add("input", 1);

            pi = await this.runtimeManager.StartProcessByKeyAsync("exclusiveGwDefaultSequenceFlow",
                map);

            query = this.taskManager
                .CreateQuery()
                .SetProcessInstanceId(pi.Id)
                .SetState(TaskState.Active);
            tasks = await query.ListAsync();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Input is one");
            await taskManager.CompleteAsync(tasks[0].Id);
            this.AssertProcessEnded(pi.Id);

            //3. Set input = 3, to take 'flow3'
            map = new Dictionary<string, object>();
            map.Add("input", 3);

            pi = await this.runtimeManager.StartProcessByKeyAsync("exclusiveGwDefaultSequenceFlow",
                map);

            query = this.taskManager
                .CreateQuery()
                .SetProcessInstanceId(pi.Id)
                .SetState(TaskState.Active);
            tasks = await query.ListAsync();
            Assert.True(tasks.Count == 1);
            Assert.True(tasks[0].Name == "Input is three");
            await taskManager.CompleteAsync(tasks[0].Id);
            this.AssertProcessEnded(pi.Id);

            //this.Commit();
        }
    }
}
