using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialSubProcessEmptyCollectionTestCase : BpmtkTestCase
    {
        public SequentialSubProcessEmptyCollectionTestCase(ITestOutputHelper output) : base(output)
        {

        }

        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialSubprocessEmptyCollection.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("collection", new string[0] { });
            //map.Add("nrOfLoops", 5);

            var pi = this.runtimeService.StartProcessInstanceByKey("testSequentialSubProcessEmptyCollection",
                map);

            var query = this.taskService.CreateQuery()
                .SetState(Tasks.TaskState.Active)
                .SetProcessInstanceId(pi.Id);

            var tasks = query.List();
            Assert.True(tasks.Count == 0);

            while(tasks.Count > 0)
            {
                //Assert.True(1 == tasks.Count);

                //Assert.True("task one" == tasks[0].Name);

                taskService.Complete(tasks[0].Id);
                tasks = query.List();
            }

            this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
