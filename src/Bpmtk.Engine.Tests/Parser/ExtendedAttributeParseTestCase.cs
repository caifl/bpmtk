using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Bpmtk.Bpmn2;
using Bpmtk.Engine.Bpmn2.Parser;

namespace Bpmtk.Engine.Tests.Parser
{
    public class ExtendedAttributeParseTestCase : BpmtkTestCase
    {
        protected BpmnParser bpmnParser;

        public ExtendedAttributeParseTestCase(ITestOutputHelper output) : base(output)
        {
            this.bpmnParser = BpmnParser.Create();
        }

        [Fact]
        public virtual void Execute()
        {
            var ms = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Engine.Tests.Parser.ExtendedAttributes.bpmn.xml");
            Assert.NotNull(ms);

            var results = this.bpmnParser.Parse(ms);
            Assert.NotNull(results);

            var process = results.Definitions.RootElements.OfType<Process>().SingleOrDefault();
            Assert.NotNull(process);

            var task = process.FlowElements.OfType<Task>().SingleOrDefault();
            Assert.NotNull(task);

            Assert.True(task.Attributes.Count == 2);

            var attr = task.Attributes[1];
            Assert.True(attr.Name == "taskImpl");
            Assert.True(attr.Value == "ddddd");

            attr = task.Attributes[0];
            Assert.True(attr.Name == "taskName");
            Assert.True(attr.Value == "TASK-${assignee}-001");

            var startEvent = process.FlowElements.OfType<StartEvent>().SingleOrDefault();
            Assert.NotNull(startEvent);

            Assert.True(startEvent.Scripts.Count == 1);
        }
    }
}
