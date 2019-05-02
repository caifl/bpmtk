using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Event
{
    public class BpmnInlineScriptEventTestCase : BpmtkTestCase
    {
        public BpmnInlineScriptEventTestCase(ITestOutputHelper output) : base(output)
        {

        }

      
        [Fact] public async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Event.BpmnInlineScriptEventTestCase.bpmn.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("BpmnInlineScriptEventTestCase");

            var variables = await this.runtimeManager.GetVariablesAsync(pi.Id);
            var greeting = variables["greeting"];
            Assert.True("hello bpmtk!".Equals(greeting));

            var taskEnded = variables["taskEnded"];

            Assert.True(true.Equals(taskEnded));

            this.AssertProcessEnded(pi.Id);

            //this.Commit();
        }
    }
}
