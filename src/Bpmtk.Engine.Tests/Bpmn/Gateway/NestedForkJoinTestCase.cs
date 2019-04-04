using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Bpmn.Gateway
{
    public class NestedForkJoinTestCase : BpmtkTestCase
    {
        public NestedForkJoinTestCase(ITestOutputHelper output) : base(output)
        {
        }

        public override void Execute()
        {
            this.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.Gateway.ParallelGatewayTest.testNestedForkJoin.bpmn20.xml");

            var pi = this.runtimeService.StartProcessInstanceByKey("nestedForkJoin");

            this.unitOfWork.Commit();
        }
    }
}
