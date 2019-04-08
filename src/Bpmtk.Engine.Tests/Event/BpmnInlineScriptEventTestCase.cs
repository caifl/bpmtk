using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Event
{
    public class BpmnInlineScriptEventTestCase : BpmtkTestCase
    {
        public BpmnInlineScriptEventTestCase(ITestOutputHelper output) : base(output)
        {
        }

      
        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Event.BpmnInlineScriptEventTestCase.bpmn.xml");

            var pi = this.runtimeService.StartProcessInstanceByKey("BpmnInlineScriptEventTestCase");

            var greeting = pi.GetVariable("greeting");
            Assert.True("hello bpmtk!".Equals(greeting));

            var taskEnded = pi.GetVariable("taskEnded");

            Assert.True(true.Equals(taskEnded));

            this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
