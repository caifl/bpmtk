using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.UserTask
{
    public class AssignTaskByVariableTestCase : BpmtkTestCase
    {
        public AssignTaskByVariableTestCase(ITestOutputHelper output) : base(output)
        {
        }

      
        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Bpmn.UserTask.AssignTaskByVariableTestCase.bpmn.xml");

            var pi = this.runtimeService.StartProcessInstanceByKey("AssignTaskByVariableTestCase");

            var query = this.taskService.CreateQuery()
                .SetProcessInstanceId(pi.Id);

            var tasks = query.List();
            Assert.True(tasks.Count == 1);

            Assert.True(tasks[0].AssigneeId == 1);

            //this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
