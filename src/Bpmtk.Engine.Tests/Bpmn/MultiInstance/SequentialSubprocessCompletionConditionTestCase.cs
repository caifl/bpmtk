using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialSubprocessCompletionConditionTestCase : BpmtkTestCase
    {
        public SequentialSubprocessCompletionConditionTestCase(ITestOutputHelper output) : base(output)
        {

        }

        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubProcessCompletionCondition.bpmn20.xml");

            var map = new Dictionary<string, object>();
            //map.Add("collection", new string[0] { });
            //map.Add("nrOfLoops", 5);

            var pi = this.runtimeService.StartProcessInstanceByKey("miSequentialSubprocessCompletionCondition",
                map);

            var query = this.taskService.CreateQuery()
                .SetState(Tasks.TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            //var tasks = query.List();
            //Assert.True(tasks.Count == 0);

            //while(tasks.Count > 0)
            //{
            //    //Assert.True(1 == tasks.Count);

            //    //Assert.True("task one" == tasks[0].Name);

            //    taskService.Complete(tasks[0].Id);
            //    tasks = query.List();
            //}

            for (int i = 0; i < 3; i++)
            {
                var tasks = query.List().OrderBy(x => x.Name).ToList();
                Assert.True(2 == tasks.Count);

                Assert.True("task one" == tasks[0].Name);
                Assert.True("task two" == tasks[1].Name);

                taskService.Complete(tasks[0].Id);
                taskService.Complete(tasks[1].Id);
            }

            this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
