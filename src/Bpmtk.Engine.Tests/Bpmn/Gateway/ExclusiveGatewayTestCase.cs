using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.Gateway
{
    public class ExclusiveGatewayTestCase : BpmtkTestCase
    {
        public ExclusiveGatewayTestCase(ITestOutputHelper output) : base(output)
        {

        }

        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Gateway.ExclusiveGatewayTest.testDefaultSequenceFlow.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("input", 3);

            var pi = this.runtimeService.StartProcessInstanceByKey("exclusiveGwDefaultSequenceFlow",
                map);

            //var myVar = pi.GetVariable("myVar");
            //Assert.True("test123".Equals(myVar));
            var query = this.taskService.CreateQuery().SetState(Tasks.TaskState.Active);

            var tasks = query.List();
            while(tasks.Count > 0)
            {
                this.taskService.AddUserPotentialOwner(tasks[0].Id, 1, "owner");
                this.taskService.Complete(tasks[0].Id);
                
                tasks = query.List();
            }
            //Assert.True(tasks.Count == 1);

            ////get variable from task-instance.
            //myVar = tasks[0].GetVariable("myVar");
            //Assert.True("test123".Equals(myVar));

            //this.taskService.Complete(tasks[0].Id);

            this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
