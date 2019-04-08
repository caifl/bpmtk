using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.MultiInstance
{
    public class SequentialScriptTasksTestCase : BpmtkTestCase
    {
        public SequentialScriptTasksTestCase(ITestOutputHelper output) : base(output)
        {

        }

        public override void Execute()
        {
            base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.MultiInstance.MultiInstanceTest.testSequentialScriptTasks.bpmn20.xml");

            var map = new Dictionary<string, object>();
            map.Add("sum", 0);
            map.Add("nrOfLoops", 5);

            var pi = this.runtimeService.StartProcessInstanceByKey("miSequentialScriptTask",
                map);

            var myVar = pi.GetVariable("sum");
            Assert.True(10 == Convert.ToInt32(myVar));

            var actInsts = this.runtimeService.CreateActivityQuery()
                .List();

            var list = actInsts.Where(x => x.ActivityType == "ScriptTask")
                .ToList();
            Assert.True(list.Count == 5);

            foreach(var item in list)
            {
                Assert.True(item.StartTime != null);
                Assert.True(item.State == Runtime.ExecutionState.Completed);
            }

            var tokenQuery = this.runtimeService.CreateTokenQuery();
            tokenQuery.SetProcessInstance(pi.Id);

            var tokens = tokenQuery.List();

            Assert.True(tokens.Count == 1);

            //trigger
            //this.runtimeService.Trigger(tokens[0].Id);

            //var query = this.taskService.CreateQuery().SetState(Tasks.TaskState.Active);

            //var tasks = query.List();
            //while(tasks.Count > 0)
            //{
            //    this.taskService.Complete(tasks[0].Id);
            //    tasks = query.List();
            //}
            //Assert.True(tasks.Count == 1);

            ////get variable from task-instance.
            //myVar = tasks[0].GetVariable("myVar");
            //Assert.True("test123".Equals(myVar));

            //this.taskService.Complete(tasks[0].Id);

            //this.AssertProcessInstanceEnd(pi.Id);

            this.unitOfWork.Commit();
        }
    }
}
