using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Scripting;
using Esprima;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Scripting
{
    public class JavascriptTestCase : BpmtkTestCase
    {
        //private IScriptEngine engine;

        public JavascriptTestCase(ITestOutputHelper output)
            : base(output)
        {
            //this.output = output;
            //this.engine = new JavascriptEngine();
        }

        //        [Fact]
        //        public void HelloWord()
        //        {
        //            var scope = this.engine.CreateScope(new Variables());
        //            scope.AddClrType("Customer", typeof(Customer));
        //            scope.SetValue("customer", new Customer() { Name = "张三" });
        //            scope.SetValue("bcd", "中华人码");

        //            var test = new Regex("(?:\\${)(.*?)(?:})", RegexOptions.IgnoreCase).Replace(@"dddd${customer.Name
        //}sd>>dddddd${ bcd }***********${ cg }yyyyyyyyyy", new MatchEvaluator((m) =>
        //            {
        //                var len = m.Length;
        //                var a = m.Value;
        //                var expression = a.Substring(2).TrimEnd('}').Trim();

        //                var value = this.engine.Execute(expression, scope);
        //                if (value != null)
        //                    return value.ToString();

        //                return string.Empty;
        //            }));

        //            //bool isExpression = false;
        //            //var script = this.engine.CreateCompileUnit(@" @@@@@  ", out isExpression);



        //            //var result = this.engine.ExecuteCompileUnit(script, scope);

        //            ///output.WriteLine(result.ToString());
        //        }
        public override async Task Execute()
        {
            await base.DeployBpmnModel("Bpmtk.Engine.Tests.Resources.ScriptTask.JavascriptTest.testSetVariableThroughExecutionInScript.bpmn20.xml");

            var pi = await this.runtimeManager.StartProcessByKeyAsync("setScriptVariableThroughExecution");

            var myVar = pi.GetVariable("myVar");
            Assert.True("test123".Equals(myVar));

            var tasks = this.taskManager.CreateQuery().SetState(TaskState.Active).List();
            Assert.True(tasks.Count == 1);

            //get variable from task-instance.
            myVar = tasks[0].GetVariable("myVar");
            Assert.True("test123".Equals(myVar));

            await taskManager.CompleteAsync(tasks[0].Id);

            this.AssertProcessEnded(pi.Id);

            this.unitOfWork.Commit();
        }

        class Variables : IVariableResolver
        {
            public bool Resolve(string name, out object value)
            {
                value = null;

                if (name == "amount")
                {
                    value = 99.99;
                    return true;
                }

                return true;
                //throw new NotImplementedException();
            }
        }

        class Customer
        {
            public virtual string Name
            {
                get;
                set;
            }

            public virtual string Say()
            {
                return "hello";
            }
        }
    }
}
