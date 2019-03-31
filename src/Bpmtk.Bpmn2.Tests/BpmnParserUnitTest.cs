using System;
using System.IO;
using System.Linq;
using Xunit;
using Bpmtk.Bpmn2.Parser;

namespace Bpmtk.Bpmn2.Tests
{
    public class BpmnParserUnitTest
    {
        [Fact]
        public void ParseSequentialFlow()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Bpmn2.Tests.Resources.sequential_flow.bpmn.xml");
            var parser = Bpmn2XmlParser.Create();
            var definitions = parser.Parse(stream);

            Assert.True(definitions != null);
            Assert.True(definitions.Id == "Definitions_0f2yaoj");
            Assert.True(definitions.RootElements.Count == 1);

            var process = definitions.RootElements[0] as Process;
            Assert.True(process != null);

            Assert.True(process.Id == "Process_0cyms8o");
            Assert.True(process.IsExecutable == false);

            Assert.True(process.FlowElements.Count == 5);

            var start = process.FlowElements.OfType<StartEvent>().SingleOrDefault();
            Assert.True(start != null);
            Assert.True(start.Id == "StartEvent_0busnrn");
            Assert.True(start.Name == "start");
            Assert.True(start.Outgoings.Count == 1 && start.Outgoings[0].Id == "SequenceFlow_0bbuw2i");

            var task = process.FlowElements.OfType<Task>().SingleOrDefault();
            Assert.True(task != null);
            Assert.True(task.Id == "Task_105g1f1");
            Assert.True(task.Name == "Hello Word");
            Assert.True(task.Incomings.Count == 1 && task.Incomings[0].Id == "SequenceFlow_0bbuw2i");
            Assert.True(task.Outgoings.Count == 1 && task.Outgoings[0].Id == "SequenceFlow_03g0j1u");

            var end = process.FlowElements.OfType<EndEvent>().SingleOrDefault();
            Assert.True(end != null);
            Assert.True(end.Id == "EndEvent_1ruiztz");
            Assert.True(end.Name == "end");
            Assert.True(end.Incomings.Count == 1 && end.Incomings[0].Id == "SequenceFlow_03g0j1u");
            
        }

        [Fact]
        public void ParseExclusiveGateway()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Bpmn2.Tests.Resources.exclusive_gateway.bpmn.xml");
            var parser = Bpmn2XmlParser.Create();
            var definitions = parser.Parse(stream);

            Assert.True(definitions != null);
            Assert.True(definitions.Id == "Definitions_0f2yaoj");
            Assert.True(definitions.RootElements.Count == 1);

            var process = definitions.RootElements[0] as Process;
            Assert.True(process != null);

            Assert.True(process.Id == "Process_0cyms8o");
            Assert.True(process.IsExecutable == false);

            Assert.True(process.FlowElements.Count == 10);

            var start = process.FlowElements.OfType<StartEvent>().SingleOrDefault();
            Assert.True(start != null);
            Assert.True(start.Id == "StartEvent_0busnrn");
            Assert.True(start.Name == "start");
            Assert.True(start.Outgoings.Count == 1 && start.Outgoings[0].Id == "SequenceFlow_0bbuw2i");

            //var task = process.FlowElements.OfType<Task>().SingleOrDefault();
            //Assert.True(task != null);
            //Assert.True(task.Id == "Task_105g1f1");
            //Assert.True(task.Name == "Hello Word");
            //Assert.True(task.Incomings.Count == 1 && task.Incomings[0].Id == "SequenceFlow_0bbuw2i");
            //Assert.True(task.Outgoings.Count == 1 && task.Outgoings[0].Id == "SequenceFlow_03g0j1u");

            var gateway = process.FlowElements.OfType<ExclusiveGateway>().Where(x => x.Id == "ExclusiveGateway_0hwaw8w").SingleOrDefault();
            Assert.True(gateway != null);
            //Assert.True(gateway.Id == "Task_105g1f1");
            //Assert.True(task.Name == "Hello Word");
            Assert.True(gateway.Incomings.Count == 1 && gateway.Incomings[0].Id == "SequenceFlow_0bbuw2i");
            Assert.True(gateway.Outgoings.Count == 2 && gateway.Outgoings[0].Id == "SequenceFlow_0a8j8sl");
            Assert.True(gateway.Outgoings.Count == 2 && gateway.Outgoings[1].Id == "SequenceFlow_04t8tmw");

            var end = process.FlowElements.OfType<EndEvent>().SingleOrDefault();
            Assert.True(end != null);
            Assert.True(end.Id == "EndEvent_1ruiztz");
            Assert.True(end.Name == "end");
            Assert.True(end.Incomings.Count == 2 && end.Incomings[0].Id == "SequenceFlow_03g0j1u");
            Assert.True(end.Incomings.Count == 2 && end.Incomings[1].Id == "SequenceFlow_0q6pbny");
        }

        [Fact]
        public void ParseSubProcess()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Bpmn2.Tests.Resources.sub_process.bpmn.xml");
            var parser = Bpmn2XmlParser.Create();
            var definitions = parser.Parse(stream);

            Assert.True(definitions != null);
            Assert.True(definitions.Id == "Definitions_0f2yaoj");
            Assert.True(definitions.RootElements.Count == 1);

            var process = definitions.RootElements[0] as Process;
            Assert.True(process != null);

            var subProcess = process.FlowElements.OfType<SubProcess>().SingleOrDefault();
            Assert.True(subProcess.Id == "SubProcess_0t9851s");
            Assert.True(subProcess.Name == "sub-process");
            //Assert.True(process.Id == "Process_0cyms8o");
            //Assert.True(process.IsExecutable == false);

            //Assert.True(process.FlowElements.Count == 5);

            //var start = process.FlowElements.OfType<StartEvent>().SingleOrDefault();
            //Assert.True(start != null);
            //Assert.True(start.Id == "StartEvent_0busnrn");
            //Assert.True(start.Name == "start");
            //Assert.True(start.Outgoings.Count == 1 && start.Outgoings[0].Id == "SequenceFlow_0bbuw2i");

            //var task = process.FlowElements.OfType<Task>().SingleOrDefault();
            //Assert.True(task != null);
            //Assert.True(task.Id == "Task_105g1f1");
            //Assert.True(task.Name == "Hello Word");
            //Assert.True(task.Incomings.Count == 1 && task.Incomings[0].Id == "SequenceFlow_0bbuw2i");
            //Assert.True(task.Outgoings.Count == 1 && task.Outgoings[0].Id == "SequenceFlow_03g0j1u");

            //var end = process.FlowElements.OfType<EndEvent>().SingleOrDefault();
            //Assert.True(end != null);
            //Assert.True(end.Id == "EndEvent_1ruiztz");
            //Assert.True(end.Name == "end");
            //Assert.True(end.Incomings.Count == 1 && end.Incomings[0].Id == "SequenceFlow_03g0j1u");

        }

        [Fact]
        public void ParseDataAssociation()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Bpmn2.Tests.Resources.data_association.bpmn.xml");
            var parser = Bpmn2XmlParser.Create();
            var definitions = parser.Parse(stream);

            Assert.True(definitions != null);
            Assert.True(definitions.Id == "Definitions_0f2yaoj");
            Assert.True(definitions.RootElements.Count == 1);

            var process = definitions.RootElements[0] as Process;
            Assert.True(process != null);

            //var subProcess = process.FlowElements.OfType<SubProcess>().SingleOrDefault();
            //Assert.True(subProcess.Id == "SubProcess_0t9851s");
            //Assert.True(subProcess.Name == "sub-process");
            //Assert.True(process.Id == "Process_0cyms8o");
            //Assert.True(process.IsExecutable == false);

            //Assert.True(process.FlowElements.Count == 5);

            //var start = process.FlowElements.OfType<StartEvent>().SingleOrDefault();
            //Assert.True(start != null);
            //Assert.True(start.Id == "StartEvent_0busnrn");
            //Assert.True(start.Name == "start");
            //Assert.True(start.Outgoings.Count == 1 && start.Outgoings[0].Id == "SequenceFlow_0bbuw2i");

            //var task = process.FlowElements.OfType<Task>().SingleOrDefault();
            //Assert.True(task != null);
            //Assert.True(task.Id == "Task_105g1f1");
            //Assert.True(task.Name == "Hello Word");
            //Assert.True(task.Incomings.Count == 1 && task.Incomings[0].Id == "SequenceFlow_0bbuw2i");
            //Assert.True(task.Outgoings.Count == 1 && task.Outgoings[0].Id == "SequenceFlow_03g0j1u");

            //var end = process.FlowElements.OfType<EndEvent>().SingleOrDefault();
            //Assert.True(end != null);
            //Assert.True(end.Id == "EndEvent_1ruiztz");
            //Assert.True(end.Name == "end");
            //Assert.True(end.Incomings.Count == 1 && end.Incomings[0].Id == "SequenceFlow_03g0j1u");

        }
    }
}
